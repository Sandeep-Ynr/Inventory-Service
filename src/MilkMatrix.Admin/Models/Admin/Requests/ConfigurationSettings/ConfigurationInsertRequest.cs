namespace MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings;

/// <summary>
/// Represents a request to insert a new configuration setting or tag.
/// </summary>
public class ConfigurationInsertRequest
{

    /// <summary>
    /// Gets or sets the unique identifier for the tag. If not provided, a new tag will be created.
    /// </summary>
    public string TagName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the tag. This can be a string, boolean, or flag.
    /// </summary>
    public string? TagValue { get; set; }

    /// <summary>
    /// Gets or sets a boolean value indicating whether the tag value is a boolean type.
    /// </summary>
    public bool? TagValueBool { get; set; }

    /// <summary>
    /// Gets or sets a boolean flag indicating whether the tag is active or should be skipped for the user.
    /// </summary>
    public bool? TagFlag { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this tag should be skipped for the user.
    /// </summary>
    public string? SkipForUser { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated business. If not provided, the tag will be created without a specific business association.
    /// </summary>
    public int? BusinessId { get; set; }

    /// <summary>
    /// Gets or sets the device type of the associated business. This is optional and can be used for display purposes.
    /// </summary>
    public string? DeviceType { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who created this tag. This is useful for tracking who added the tag.
    /// </summary>
    public int CreatedBy { get; set; }
}
