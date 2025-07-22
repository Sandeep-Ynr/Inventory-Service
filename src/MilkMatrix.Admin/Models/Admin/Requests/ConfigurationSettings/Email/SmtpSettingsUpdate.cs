namespace MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.Email;

/// <summary>
/// Represents a request to update existing SMTP settings in the system.
/// </summary>
public class SmtpSettingsUpdate
{

    /// <summary>
    /// Gets or sets the unique identifier for the SMTP settings.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the SMTP server address.
    /// </summary>
    public string? SmtpServer { get; set; }

    /// <summary>
    /// Gets or sets the SMTP port number.
    /// </summary>
    public int? SmtpPort { get; set; }

    /// <summary>
    /// Gets or sets the username for SMTP authentication.
    /// </summary>
    public string? SmtpUserId { get; set; }

    /// <summary>
    /// Gets or sets the password for SMTP authentication.
    /// </summary>
    public string? SmtpPassword { get; set; }

    /// <summary>
    /// Gets or sets who modified the SMTP settings.
    /// </summary>
    public int ModifyBy { get; set; }

    /// <summary>
    /// Gets or sets who modified the SMTP settings.
    /// </summary>
    public bool? IsActive { get; set; }
}
