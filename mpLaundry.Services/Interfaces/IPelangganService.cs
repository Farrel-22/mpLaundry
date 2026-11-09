using mpLaundry.Common;
using mpLaundry.DAL.ScaffoldContainer;

namespace mpLaundry.Services.Interfaces;

public interface IPelangganService
{
    Task<List<MPelanggan>> GetAllAsync(string? cari = null);

    Task<List<MPelanggan>> GetAktifAsync();

    Task<MPelanggan?> GetByIdAsync(Guid id);

    Task<ServiceResult> CreateAsync(MPelanggan pelanggan);

    Task<ServiceResult> UpdateAsync(MPelanggan pelanggan);

    Task<ServiceResult> DeleteAsync(Guid id);
}    
