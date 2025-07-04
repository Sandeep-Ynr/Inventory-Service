namespace MilkMatrix.Admin.Models.Admin.Responses.ConfigurationSettings;

/// <summary>
/// Represents the details of an SMS control configuration setting.
/// </summary>
public class SmsControlDetails
{
    /// <summary>
    /// Gets or sets the unique identifier for the SMS control configuration setting.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the SMS merchant name.
    /// </summary>
    public string SmsMerchant { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the sender ID for SMS.
    /// </summary>
    public string SenderId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the authentication key for SMS.
    /// </summary>
    public string AuthKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URL link for the SMS service.
    /// </summary>
    public string UrlLink { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the template ID for SMS messages.
    /// </summary>
    public int TemplateId { get; set; }

    /// <summary>
    /// Gets or sets the order ID associated with the SMS control configuration.
    /// </summary>
    public int OrderId { get; set; }
}
