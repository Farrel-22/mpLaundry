using System;

namespace mpLaundry.DAL.ScaffoldContainer;

public interface IAuditEntity
{
    string RefInsertedBy { get; set; }
    DateTime DtInserted { get; set; }
    string? RefUpdatedBy { get; set; }
    DateTime? DtUpdated { get; set; }
}
