using System;
using System.Collections.Generic;

namespace mpLaundry.DAL.ScaffoldContainer;

public partial class TrLaundry
{
    public Guid PkLaundryId { get; set; }

    public string TxtNoOrder { get; set; } = null!;

    public DateTime DtMasuk { get; set; }

    public DateTime DtEstimasiSelesai { get; set; }

    public DateTime? DtDiambil { get; set; }

    public Guid RefPelangganId { get; set; }

    public string TxtStatus { get; set; } = null!;

    public decimal DecTotalHarga { get; set; }

    public decimal DecBayar { get; set; }

    public bool BolLunas { get; set; }

    public string? TxtKeterangan { get; set; }

    public string RefInsertedBy { get; set; } = null!;

    public DateTime DtInserted { get; set; }

    public string? RefUpdatedBy { get; set; }

    public DateTime? DtUpdated { get; set; }

    public virtual MPelanggan RefPelanggan { get; set; } = null!;

    public virtual ICollection<TrLaundryDetail> TrLaundryDetail { get; set; } = new List<TrLaundryDetail>();
}
