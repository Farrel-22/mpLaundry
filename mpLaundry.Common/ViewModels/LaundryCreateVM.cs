using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace mpLaundry.Common.ViewModels;

public class LaundryCreateVM
{
    [DataType(DataType.Date)]
    [Display(Name = "Tanggal Masuk")]
    public DateTime TanggalMasuk { get; set; } = DateTime.Today;
    [Required(ErrorMessage = "Pelanggan wajib dipilih")]
    [Display(Name = "Pelanggan")]
    public Guid? PelangganId { get; set; }
    [Range(0, 999_999_999, ErrorMessage = "Uang muka tidak boleh negatif")]
    [Display(Name = "Uang Muka / Pembayaran Awal")]
    public decimal Bayar { get; set; }
    [StringLength(250)]
    [Display(Name = "Keterangan")]
    public string? Keterangan { get; set; }

    [MinLength(1, ErrorMessage = "Minimal harus ada 1 item layanan cucian")]
    public List<LaundryItemVM> Items { get; set; } = new();
}
public class LaundryItemVM
{
    [Required(ErrorMessage = "Layanan wajib dipilih")]
    public Guid? LayananId { get; set; }
    [Range(0.01, 1000, ErrorMessage = "Jumlah / Qty harus lebih dari 0")]
    public decimal Qty { get; set; }
    [StringLength(100)]
    public string? Catatan { get; set; }
}
