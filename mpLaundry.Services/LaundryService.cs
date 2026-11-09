using Microsoft.EntityFrameworkCore;
using mpLaundry.Common;
using mpLaundry.Common.Helpers;
using mpLaundry.Common.ViewModels;
using mpLaundry.DAL.ScaffoldContainer;
using mpLaundry.Services.Interfaces;

namespace mpLaundry.Services;

public class LaundryService(LaundryDbContext db) : ILaundryService
{
    // 1. Ambil semua transaksi dengan filter tanggal dan status
    public Task<List<TrLaundry>> GetAllAsync(DateTime? dari, DateTime? sampai, string? status)
    {
        var query = db.TrLaundry.AsNoTracking()
            .Include(l => l.RefPelanggan)
            .AsQueryable();

        if (dari.HasValue)
            query = query.Where(l => l.DtMasuk >= dari.Value.Date);

        if (sampai.HasValue)
            query = query.Where(l => l.DtMasuk < sampai.Value.Date.AddDays(1));

        if (!string.IsNullOrEmpty(status))
            query = query.Where(l => l.TxtStatus == status);

        return query.OrderByDescending(l => l.TxtNoOrder).ToListAsync();
    }

    // 2. Ambil 1 transaksi lengkap (header + pelanggan + detail cucian)
    public Task<TrLaundry?> GetByIdAsync(Guid id) =>
        db.TrLaundry.AsNoTracking()
            .Include(l => l.RefPelanggan)
            .Include(l => l.TrLaundryDetail).ThenInclude(d => d.RefLayanan)
            .FirstOrDefaultAsync(l => l.PkLaundryId == id);

    // 3. Simpan Transaksi Baru (Header & Detail)
    public async Task<ServiceResult> CreateAsync(LaundryCreateVM vm)
    {
        // A. Validasi seluruh layanan yang dipilih kasir
        var ids = vm.Items.Select(i => i.LayananId!.Value).ToList();
        var layanan = await db.MLayanan
            .Where(l => ids.Contains(l.PkLayananId))
            .ToDictionaryAsync(l => l.PkLayananId);

        foreach (var item in vm.Items)
        {
            if (!layanan.TryGetValue(item.LayananId!.Value, out var l) || !l.BolActive)
                return ServiceResult.Gagal("Layanan tidak ditemukan atau sudah tidak aktif.");

            // Validasi satuan PCS wajib bilangan bulat
            if (l.TxtSatuan == "PCS" && item.Qty != decimal.Truncate(item.Qty))
                return ServiceResult.Gagal($"Qty {l.TxtNamaLayanan} harus bilangan bulat (satuan PCS).");

            // Validasi satuan KG minimal 1
            if (l.TxtSatuan == "KG" && item.Qty < 1)
                return ServiceResult.Gagal($"Minimal 1 kg untuk layanan {l.TxtNamaLayanan}.");
        }

        // B. Susun Header Nota
        var order = new TrLaundry
        {
            PkLaundryId = Guid.NewGuid(),
            TxtNoOrder = await BuatNoOrderAsync(vm.TanggalMasuk), // Format: LD-yyyyMMdd-0001
            DtMasuk = vm.TanggalMasuk,
            RefPelangganId = vm.PelangganId!.Value,
            TxtStatus = StatusLaundry.Diterima,
            TxtKeterangan = vm.Keterangan
        };

        // C. Susun Baris-Baris Cucian (Detail)
        foreach (var item in vm.Items)
        {
            var l = layanan[item.LayananId!.Value];
            order.TrLaundryDetail.Add(new TrLaundryDetail
            {
                PkLaundryDetailId = Guid.NewGuid(),
                RefLayananId = l.PkLayananId,
                DecQty = item.Qty,
                DecHargaSatuan = l.DecHarga, // Snapshot harga saat transaksi
                DecSubtotal = Math.Round(l.DecHarga * item.Qty, 2),
                TxtCatatan = item.Catatan
            });
        }

        // Hitung total harga nota
        order.DecTotalHarga = order.TrLaundryDetail.Sum(d => d.DecSubtotal);

        // Rumus Estimasi Selesai Terlama (Mengikuti layanan paling lama)
        var hariTerlama = vm.Items.Max(i => layanan[i.LayananId!.Value].IntEstimasiHari);
        order.DtEstimasiSelesai = vm.TanggalMasuk.Date.AddDays(hariTerlama);

        // Validasi uang muka & status lunas
        if (vm.Bayar > order.DecTotalHarga)
            return ServiceResult.Gagal($"Uang muka tidak boleh melebihi total ({Format.Rupiah(order.DecTotalHarga)}).");

        order.DecBayar = vm.Bayar;
        order.BolLunas = vm.Bayar == order.DecTotalHarga;

        // D. Simpan Header & Detail sekaligus dalam 1 database transaction
        db.TrLaundry.Add(order);
        await db.SaveChangesAsync();

        return ServiceResult.Ok($"Order {order.TxtNoOrder} berhasil disimpan.", order.PkLaundryId);
    }

    // 4. Majukan Status Order 1 Langkah (Diterima -> Diproses -> Selesai -> Diambil)
    public async Task<ServiceResult> UpdateStatusAsync(Guid id)
    {
        var order = await db.TrLaundry.FindAsync(id);
        if (order is null)
            return ServiceResult.Gagal("Order tidak ditemukan.");

        var statusBerikutnya = StatusLaundry.Berikutnya(order.TxtStatus);
        if (statusBerikutnya is null)
            return ServiceResult.Gagal("Order sudah berstatus Diambil.");

        // Aturan: Tidak boleh Diambil jika belum lunas
        if (statusBerikutnya == StatusLaundry.Diambil && !order.BolLunas)
            return ServiceResult.Gagal("Order belum lunas, tidak bisa diambil pelanggan.");

        order.TxtStatus = statusBerikutnya;

        // Catat tanggal diambil jika status berubah jadi Diambil
        if (statusBerikutnya == StatusLaundry.Diambil)
            order.DtDiambil = DateTime.Now;

        await db.SaveChangesAsync();
        return ServiceResult.Ok($"Status order {order.TxtNoOrder} berhasil diubah menjadi {statusBerikutnya}.");
    }

    // 5. Pembayaran Sisa Tagihan (Pelunasan)
    public async Task<ServiceResult> BayarAsync(Guid id, decimal jumlah)
    {
        if (jumlah <= 0)
            return ServiceResult.Gagal("Jumlah bayar harus lebih dari 0.");

        var order = await db.TrLaundry.FindAsync(id);
        if (order is null)
            return ServiceResult.Gagal("Order tidak ditemukan.");

        var sisaTagihan = order.DecTotalHarga - order.DecBayar;
        if (sisaTagihan <= 0)
            return ServiceResult.Gagal("Order ini sudah lunas.");

        if (jumlah > sisaTagihan)
            return ServiceResult.Gagal($"Jumlah bayar melebihi sisa tagihan ({Format.Rupiah(sisaTagihan)}).");

        order.DecBayar += jumlah;
        order.BolLunas = (order.DecBayar == order.DecTotalHarga);

        await db.SaveChangesAsync();
        return ServiceResult.Ok(order.BolLunas
            ? "Pembayaran diterima. Order dinyatakan LUNAS."
            : $"Pembayaran diterima. Sisa tagihan: {Format.Rupiah(sisaTagihan - jumlah)}.");
    }

    // 6. Hapus Order (Hanya jika status masih Diterima)
    public async Task<ServiceResult> DeleteAsync(Guid id)
    {
        var order = await db.TrLaundry.FindAsync(id);
        if (order is null)
            return ServiceResult.Gagal("Order tidak ditemukan.");

        if (order.TxtStatus != StatusLaundry.Diterima)
            return ServiceResult.Gagal($"Order berstatus {order.TxtStatus} tidak boleh dihapus.");

        db.TrLaundry.Remove(order); // Otomatis cascade menghapus baris di detail
        await db.SaveChangesAsync();

        return ServiceResult.Ok($"Order {order.TxtNoOrder} berhasil dihapus.");
    }

    // 7. Helper Pembuatan Nomor Order Otomatis (LD-yyyyMMdd-NNNN)
    private async Task<string> BuatNoOrderAsync(DateTime tanggal)
    {
        var prefix = $"LD-{tanggal:yyyyMMdd}-";

        var terakhir = await db.TrLaundry
            .Where(l => l.TxtNoOrder.StartsWith(prefix))
            .OrderByDescending(l => l.TxtNoOrder)
            .Select(l => l.TxtNoOrder)
            .FirstOrDefaultAsync();

        var urut = terakhir is null ? 1 : int.Parse(terakhir[^4..]) + 1;
        return $"{prefix}{urut:D4}";
    }
}