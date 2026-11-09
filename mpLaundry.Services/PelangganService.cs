using Microsoft.EntityFrameworkCore;
using mpLaundry.Common;
using mpLaundry.DAL.ScaffoldContainer;
using mpLaundry.Services.Interfaces;

namespace mpLaundry.Services;

public class PelangganService(LaundryDbContext db) : IPelangganService
{
    // 1. Ambil semua pelanggan (bisa difilter pencarian kode/nama/telepon)
    public Task<List<MPelanggan>> GetAllAsync(string? cari = null)
    {
        var query = db.MPelanggan.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(cari))
        {
            query = query.Where(p => p.TxtKodePelanggan.Contains(cari)
                                  || p.TxtNama.Contains(cari)
                                  || p.TxtNoTelepon.Contains(cari));
        }

        return query.OrderBy(p => p.TxtNama).ToListAsync();
    }

    // 2. Ambil pelanggan aktif saja (untuk dropdown form transaksi order)
    public Task<List<MPelanggan>> GetAktifAsync() =>
        db.MPelanggan.AsNoTracking()
            .Where(p => p.BolActive)
            .OrderBy(p => p.TxtNama)
            .ToListAsync();

    // 3. Ambil satu pelanggan berdasarkan ID
    public Task<MPelanggan?> GetByIdAsync(Guid id) =>
        db.MPelanggan.AsNoTracking().FirstOrDefaultAsync(p => p.PkPelangganId == id);

    // 4. Tambah pelanggan baru (Aturan: Kode harus unik)
    public async Task<ServiceResult> CreateAsync(MPelanggan pelanggan)
    {
        if (await KodeSudahAdaAsync(pelanggan.TxtKodePelanggan, null))
            return ServiceResult.Gagal("Kode pelanggan sudah digunakan.");

        pelanggan.PkPelangganId = Guid.NewGuid();
        pelanggan.BolActive = true;

        db.MPelanggan.Add(pelanggan);
        await db.SaveChangesAsync();

        return ServiceResult.Ok("Pelanggan berhasil ditambahkan.", pelanggan.PkPelangganId);
    }

    // 5. Ubah data pelanggan (Aturan: Kode unik kecuali dirinya sendiri)
    public async Task<ServiceResult> UpdateAsync(MPelanggan pelanggan)
    {
        var data = await db.MPelanggan.FindAsync(pelanggan.PkPelangganId);
        if (data is null)
            return ServiceResult.Gagal("Pelanggan tidak ditemukan.");

        if (await KodeSudahAdaAsync(pelanggan.TxtKodePelanggan, pelanggan.PkPelangganId))
            return ServiceResult.Gagal("Kode pelanggan sudah digunakan.");

        // Salin hanya field yang boleh diubah; kolom audit diurus otomatis oleh DbContext
        data.TxtKodePelanggan = pelanggan.TxtKodePelanggan;
        data.TxtNama = pelanggan.TxtNama;
        data.TxtAlamat = pelanggan.TxtAlamat;
        data.TxtNoTelepon = pelanggan.TxtNoTelepon;
        data.TxtEmail = pelanggan.TxtEmail;
        data.BolActive = pelanggan.BolActive;

        await db.SaveChangesAsync();

        return ServiceResult.Ok("Pelanggan berhasil diubah.", data.PkPelangganId);
    }

    // 6. Hapus pelanggan (Aturan: Jika sudah punya riwayat order, tolak hapus!)
    public async Task<ServiceResult> DeleteAsync(Guid id)
    {
        var data = await db.MPelanggan.FindAsync(id);
        if (data is null)
            return ServiceResult.Gagal("Pelanggan tidak ditemukan.");

        // Cek apakah pelanggan pernah transaksi laundry
        if (await db.TrLaundry.AnyAsync(o => o.RefPelangganId == id))
            return ServiceResult.Gagal(
                "Pelanggan sudah memiliki riwayat order laundry. Nonaktifkan saja sebagai gantinya.");

        db.MPelanggan.Remove(data);
        await db.SaveChangesAsync();

        return ServiceResult.Ok("Pelanggan berhasil dihapus.");
    }

    // Helper pengecekan duplikasi kode
    private Task<bool> KodeSudahAdaAsync(string kode, Guid? kecualiId) =>
        db.MPelanggan.AnyAsync(p => p.TxtKodePelanggan == kode
                                 && p.PkPelangganId != kecualiId);
}
