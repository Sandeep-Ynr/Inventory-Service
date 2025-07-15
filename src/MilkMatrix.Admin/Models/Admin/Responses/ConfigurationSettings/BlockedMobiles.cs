using MilkMatrix.Core.Attributes;

namespace MilkMatrix.Admin.Models.Admin.Responses.ConfigurationSettings;

/// <summary>
/// Represents a blocked mobile entry in the system.
/// </summary>
public class BlockedMobiles
{
    /// <summary>
    /// Gets or sets the unique identifier for the blocked mobile entry.
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Gets or sets the mobile number that is blocked.
    /// </summary>
    [GlobalSearch]
    public string Mobile { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the name associated with the blocked mobile number.
    /// </summary>
    [GlobalSearch]
    public string ContactName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the business identifier associated with the blocked mobile number.
    /// </summary>
    public int? BusinessId { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the blocked mobile entry is active.
    /// </summary>
    public bool IsActive { get; set; }
}
