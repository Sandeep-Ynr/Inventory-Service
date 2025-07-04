namespace MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings;

/// <summary>
/// Represents the model for inserting new SMS control settings in the administration panel.
/// </summary>
public class SmsControlInsert
{
    /// <summary>
    /// Gets or sets the SMS merchant name.
    /// </summary>
    public string SmsMerchant { get; set; }
    /// <summary>
    /// Gets or sets the sender ID for SMS.
    /// </summary>
    public string SenderId { get; set; }
    /// <summary>
    /// Gets or sets the authentication key for SMS.
    /// </summary>
    public string AuthKey { get; set; }
    /// <summary>
    /// Gets or sets the URL link for SMS service.
    /// </summary>
    public string UrlLink { get; set; }
    /// <summary>
    /// Gets or sets the template ID for SMS.
    /// </summary>
    public int? TemplateId { get; set; } = 1;
    /// <summary>
    /// Gets or sets the order ID associated with the SMS control.
    /// </summary>
    public int OrderId { get; set; } = 1;

    /// <summary>
    /// Gets or sets the identifier of the user who created the SMS control settings.
    /// </summary>
    public int CreatedBy { get; set; }
}
