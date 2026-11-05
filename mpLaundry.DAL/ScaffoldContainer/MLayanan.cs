using System;
using System.Collections.Generic;

namespace mpLaundry.DAL.ScaffoldContainer;

public partial class MLayanan
{
    public Guid PkLayananId { get; set; }

    public string TxtKodeLayanan { get; set; } = null!;

    public string TxtNamaLayanan { get; set; } = null!;

    public string TxtSatuan { get; set; } = null!;

    public decimal DecHarga { get; set; }

    public int IntEstimasiHari { get; set; }

    public bool BolActive { get; set; }

    public string RefInsertedBy { get; set; } = null!;

    public DateTime DtInserted { get; set; }

    public string? RefUpdatedBy { get; set; }

    public DateTime? DtUpdated { get; set; }

    public virtual ICollection<TrLaundryDetail> TrLaundryDetail { get; set; } = new List<TrLaundryDetail>();
}
