using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace mpLaundry.DAL.ScaffoldContainer;

[ModelMetadataType(typeof(MLayananMetadata))]
public partial class MLayanan : IAuditEntity { }

public class MLayananMetadata
{
    [Required(ErrorMessage = "Kode layanan wajib diisi"), StringLength(10)]
    [Display(Name = "Kode Layanan")]
    public string TxtKodeLayanan { get; set; } = null!;

    [Required(ErrorMessage = "Nama layanan wajib diisi"), StringLength(100)]
    [Display(Name = "Nama Layanan")]
    public string TxtNamaLayanan { get; set; } = null!;

    [Required]
    [RegularExpression("^(KG|PCS)$", ErrorMessage = "Satuan harus KG atau PCS")]
    [Display(Name = "Satuan")]
    public string TxtSatuan { get; set; } = null!;

    [Range(1, 999_999_999, ErrorMessage = "Harga harus lebih dari 0")]
    [Display(Name = "Harga")]
    public decimal DecHarga { get; set; }

    [Range(1, 30, ErrorMessage = "Estimasi 1 s.d. 30 hari")]
    [Display(Name = "Estimasi (hari)")]
    public int IntEstimasiHari { get; set; }

    [Display(Name = "Aktif")]
    public bool BolActive { get; set; }

    [BindNever, ValidateNever] public string RefInsertedBy { get; set; } = null!;
    [BindNever, ValidateNever] public DateTime DtInserted { get; set; }
    [BindNever, ValidateNever] public string? RefUpdatedBy { get; set; }
    [BindNever, ValidateNever] public DateTime? DtUpdated { get; set; }
}