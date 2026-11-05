namespace mpLaundry.Common.Abstract;

/// <summary>
/// Base class for all persistent entities.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Public unique identifier.
    /// </summary>
    public Guid Uid { get; set; } = Guid.NewGuid();

    /// <summary>
    /// UTC timestamp when the record was created.
    /// </summary>
    public DateTime DtCreated { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User who created the record.
    /// </summary>
    public string? TxtCreatedBy { get; set; }

    /// <summary>
    /// UTC timestamp of the last update.
    /// </summary>
    public DateTime? DtUpdated { get; set; }

    /// <summary>
    /// User who last updated the record.
    /// </summary>
    public string? TxtUpdatedBy { get; set; }

    /// <summary>
    /// Whether the record is active.
    /// </summary>
    public bool BitActive { get; set; } = true;
}
