using MilkMatrix.Core.Attributes;
using MilkMatrix.Core.Entities.Common;

namespace MilkMatrix.Admin.Models.Admin.Responses.ConfigurationSettings;

/// <summary>
/// Represents a tag or setting associated with a business.
/// </summary>
public class ConfigurationDetails : Audit
{ 

    /// <summary>
    /// Gets or sets the unique identifier for the tag.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the tag.
    /// </summary>
    [GlobalSearch]
    public string TagName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the tag.
    /// </summary>
    public string TagValue { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this tag should be skipped for the user.
    /// </summary>
    public string SkipForUser { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the associated business.
    /// </summary>
    public int BusinessId { get; set; }

    /// <summary>
    /// Gets or sets the name of the associated business.
    /// </summary>
    [GlobalSearch]
    public string BusinessName { get; set; } = string.Empty; 

    public bool? IsActive { get; set; }
}
