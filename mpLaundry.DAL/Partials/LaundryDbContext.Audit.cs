using Microsoft.EntityFrameworkCore;

namespace mpLaundry.DAL.ScaffoldContainer;

public partial class LaundryDbContext
{
    public const string SystemUserId = "00000000-0000-0000-0000-000000000001";

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        foreach (var entry in ChangeTracker.Entries<IAuditEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.RefInsertedBy = SystemUserId;
                entry.Entity.DtInserted = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(IAuditEntity.RefInsertedBy)).IsModified = false;
                entry.Property(nameof(IAuditEntity.DtInserted)).IsModified = false;
                entry.Entity.RefUpdatedBy = SystemUserId;
                entry.Entity.DtUpdated = now;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
