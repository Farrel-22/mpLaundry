namespace mpLaundry.Common.Abstract;

public abstract class BaseEntity
{
    public Guid Uid { get; set; } = Guid.NewGuid();

    public DateTime DtCreated { get; set; } = DateTime.UtcNow;

    public string? TxtCreatedBy { get; set; }

    public DateTime? DtUpdated { get; set; }

    public string? TxtUpdatedBy { get; set; }

    public bool BitActive { get; set; } = true;
}
