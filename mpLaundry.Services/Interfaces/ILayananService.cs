using mpLaundry.Common;
using mpLaundry.DAL.ScaffoldContainer;

namespace mpLaundry.Services.Interfaces;

public interface ILayananService
{
    Task<List<MLayanan>> GetAllAsync(string? cari = null);

    Task<List<MLayanan>> GetAktifAsync();

    Task<MLayanan?> GetByIdAsync(Guid id);

    Task<ServiceResult> CreateAsync(MLayanan layanan);

    Task<ServiceResult> UpdateAsync(MLayanan layanan);

    Task<ServiceResult> DeleteAsync(Guid id);
}