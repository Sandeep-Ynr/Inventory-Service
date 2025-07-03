using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Api.Models.Request.Admin.ConfigurationSettings;

/// <summary>
/// Represents the model for updating SMS control settings in the administration panel.
/// </summary>
public class SmsControlUpdateModel
{
    [Required]
    /// <summary>
    /// Gets or sets the unique identifier for the SMS control configuration setting.
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Gets or sets the SMS merchant name.
    /// </summary>
    public string? SmsMerchant { get; set; }
    /// <summary>
    /// Gets or sets the sender ID for SMS.
    /// </summary>
    public string? SenderId { get; set; }
    /// <summary>
    /// Gets or sets the authentication key for SMS.
    /// </summary>
    public string? AuthKey { get; set; }
    /// <summary>
    /// Gets or sets the URL link for SMS service.
    /// </summary>
    public string? UrlLink { get; set; }
    /// <summary>
    /// Gets or sets the template ID for SMS.
    /// </summary>
    public int? TemplateId { get; set; }
    /// <summary>
    /// Gets or sets the order ID associated with the SMS control.
    /// </summary>
    public int? OrderId { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the SMS control is active.
    /// </summary>
    public bool? IsActive { get; set; }
}
