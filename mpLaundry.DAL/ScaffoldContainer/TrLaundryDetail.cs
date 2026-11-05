using System;
using System.Collections.Generic;

namespace mpLaundry.DAL.ScaffoldContainer;

public partial class TrLaundryDetail
{
    public Guid PkLaundryDetailId { get; set; }

    public Guid RefLaundryId { get; set; }

    public Guid RefLayananId { get; set; }

    public decimal DecQty { get; set; }

    public decimal DecHargaSatuan { get; set; }

    public decimal DecSubtotal { get; set; }

    public string? TxtCatatan { get; set; }

    public string RefInsertedBy { get; set; } = null!;

    public DateTime DtInserted { get; set; }

    public string? RefUpdatedBy { get; set; }

    public DateTime? DtUpdated { get; set; }

    public virtual TrLaundry RefLaundry { get; set; } = null!;

    public virtual MLayanan RefLayanan { get; set; } = null!;
}
