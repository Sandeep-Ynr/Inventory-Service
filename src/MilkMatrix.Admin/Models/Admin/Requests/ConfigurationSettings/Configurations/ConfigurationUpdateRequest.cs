namespace MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.Configurations;

/// <summary>
/// Represents a request to update an existing configuration setting or tag.
/// </summary>
public class ConfigurationUpdateRequest
{
    /// <summary>
    /// Gets or sets the unique identifier for the tag. This is used to identify which tag to update.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the value of the tag. This can be a string, boolean, or flag.
    /// </summary>
    public string? TagValue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this tag should be skipped for the user.
    /// </summary>
    public string? SkipForUser { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated business. If not provided, the tag will be created without a specific business association.
    /// </summary>
    public int? BusinessId { get; set; }


    /// <summary>
    /// Gets or sets the unique identifier of the user who created this tag. This is useful for tracking who added the tag.
    /// </summary>
    public int ModifyBy { get; set; }

    public bool? IsActive { get; set; }
}
