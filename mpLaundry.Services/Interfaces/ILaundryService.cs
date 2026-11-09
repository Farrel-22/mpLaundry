using mpLaundry.Common;
using mpLaundry.Common.ViewModels;
using mpLaundry.DAL.ScaffoldContainer;

namespace mpLaundry.Services.Interfaces;

public interface ILaundryService
{
    // 1. Mengambil semua daftar transaksi (bisa difilter tanggal masuk & status)
    Task<List<TrLaundry>> GetAllAsync(DateTime? dari, DateTime? sampai, string? status);

    // 2. Mengambil 1 order lengkap (termasuk nama pelanggan dan rincian cucian)
    Task<TrLaundry?> GetByIdAsync(Guid id);

    // 3. Simpan transaksi baru dari form kasir (Header + Detail)
    Task<ServiceResult> CreateAsync(LaundryCreateVM vm);

    // 4. Memajukan alur status 1 langkah: Diterima -> Diproses -> Selesai -> Diambil
    Task<ServiceResult> UpdateStatusAsync(Guid id);

    // 5. Pembayaran / pelunasan sisa tagihan cucian
    Task<ServiceResult> BayarAsync(Guid id, decimal jumlah);

    // 6. Batalkan / hapus order (Hanya boleh jika status masih 'Diterima')
    Task<ServiceResult> DeleteAsync(Guid id);
}
