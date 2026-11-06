using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace mpLaundry.DAL.ScaffoldContainer;

[ModelMetadataType(typeof(MPelangganMetadata))]
public partial class MPelanggan : IAuditEntity { }

public class MPelangganMetadata
{
    [Required(ErrorMessage = "Kode pelanggan wajib diisi"), StringLength(10)]
    [Display(Name = "Kode Pelanggan")]
    public string TxtKodePelanggan { get; set; } = null!;

    [Required(ErrorMessage = "Nama pelanggan wajib diisi"), StringLength(100)]
    [Display(Name = "Nama Pelanggan")]
    public string TxtNama { get; set; } = null!;

    [StringLength(200)]
    [Display(Name = "Alamat")]
    public string? TxtAlamat { get; set; }

    [Required(ErrorMessage = "Nomor telepon wajib diisi"), StringLength(20)]
    [Display(Name = "No. Telepon")]
    public string TxtNoTelepon { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Format email tidak valid"), StringLength(100)]
    [Display(Name = "Email")]
    public string? TxtEmail { get; set; }

    [Display(Name = "Aktif")]
    public bool BolActive { get; set; }

    [BindNever, ValidateNever] public string RefInsertedBy { get; set; } = null!;
    [BindNever, ValidateNever] public DateTime DtInserted { get; set; }
    [BindNever, ValidateNever] public string? RefUpdatedBy { get; set; }
    [BindNever, ValidateNever] public DateTime? DtUpdated { get; set; }
}