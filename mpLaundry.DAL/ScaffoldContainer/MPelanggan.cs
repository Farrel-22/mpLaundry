using System;
using System.Collections.Generic;

namespace mpLaundry.DAL.ScaffoldContainer;

public partial class MPelanggan
{
    public Guid PkPelangganId { get; set; }

    public string TxtKodePelanggan { get; set; } = null!;

    public string TxtNama { get; set; } = null!;

    public string? TxtAlamat { get; set; }

    public string TxtNoTelepon { get; set; } = null!;

    public string? TxtEmail { get; set; }

    public bool BolActive { get; set; }

    public string RefInsertedBy { get; set; } = null!;

    public DateTime DtInserted { get; set; }

    public string? RefUpdatedBy { get; set; }

    public DateTime? DtUpdated { get; set; }

    public virtual ICollection<TrLaundry> TrLaundry { get; set; } = new List<TrLaundry>();
}
