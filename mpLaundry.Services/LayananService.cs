using Microsoft.EntityFrameworkCore;
using mpLaundry.Common;
using mpLaundry.DAL.ScaffoldContainer;
using mpLaundry.Services.Interfaces;

namespace mpLaundry.Services;

public class LayananService(LaundryDbContext db) : ILayananService
{
    public Task<List<MLayanan>> GetAllAsync(string? cari = null)
    {
        var query = db.MLayanan.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(cari))
            query = query.Where(l => l.TxtKodeLayanan.Contains(cari)
                                  || l.TxtNamaLayanan.Contains(cari));
        return query.OrderBy(l => l.TxtNamaLayanan).ToListAsync();
    }

    public Task<List<MLayanan>> GetAktifAsync() =>
        db.MLayanan.AsNoTracking()
            .Where(l => l.BolActive)
            .OrderBy(l => l.TxtNamaLayanan)
            .ToListAsync();

    public Task<MLayanan?> GetByIdAsync(Guid id) =>
        db.MLayanan.AsNoTracking().FirstOrDefaultAsync(l => l.PkLayananId == id);

    public async Task<ServiceResult> CreateAsync(MLayanan layanan)
    {
        if (await KodeSudahAdaAsync(layanan.TxtKodeLayanan, null))
            return ServiceResult.Gagal("Kode layanan sudah digunakan.");

        layanan.PkLayananId = Guid.NewGuid();
        layanan.BolActive = true;

        db.MLayanan.Add(layanan);
        await db.SaveChangesAsync();
        return ServiceResult.Ok("Layanan berhasil ditambahkan.", layanan.PkLayananId);
    }

    public async Task<ServiceResult> UpdateAsync(MLayanan layanan)
    {
        var data = await db.MLayanan.FindAsync(layanan.PkLayananId);
        if (data is null)
            return ServiceResult.Gagal("Layanan tidak ditemukan.");

        if (await KodeSudahAdaAsync(layanan.TxtKodeLayanan, layanan.PkLayananId))
            return ServiceResult.Gagal("Kode layanan sudah digunakan.");

        // Salin hanya field yang boleh diubah; kolom audit diurus DbContext
        data.TxtKodeLayanan = layanan.TxtKodeLayanan;
        data.TxtNamaLayanan = layanan.TxtNamaLayanan;
        data.TxtSatuan = layanan.TxtSatuan;
        data.DecHarga = layanan.DecHarga;
        data.IntEstimasiHari = layanan.IntEstimasiHari;
        data.BolActive = layanan.BolActive;

        await db.SaveChangesAsync();
        return ServiceResult.Ok("Layanan berhasil diubah.", data.PkLayananId);
    }

    public async Task<ServiceResult> DeleteAsync(Guid id)
    {
        var data = await db.MLayanan.FindAsync(id);
        if (data is null)
            return ServiceResult.Gagal("Layanan tidak ditemukan.");

        // Layanan yang sudah dipakai transaksi tidak boleh dihapus
        if (await db.TrLaundryDetail.AnyAsync(d => d.RefLayananId == id))
            return ServiceResult.Gagal(
                "Layanan sudah dipakai transaksi. Nonaktifkan saja.");

        db.MLayanan.Remove(data);
        await db.SaveChangesAsync();
        return ServiceResult.Ok("Layanan berhasil dihapus.");
    }

    private Task<bool> KodeSudahAdaAsync(string kode, Guid? kecualiId) =>
        db.MLayanan.AnyAsync(l => l.TxtKodeLayanan == kode
                               && l.PkLayananId != kecualiId);
}
