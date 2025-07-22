namespace MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.Email;

/// <summary>
/// Represents a request to insert new SMTP settings into the system.
/// </summary>
public class SmtpSettingsInsert
{
    /// <summary>
    /// Gets or sets the SMTP server address.
    /// </summary>
    public string SmtpServer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SMTP port number.
    /// </summary>
    public int SmtpPort { get; set; }

    /// <summary>
    /// Gets or sets the username for SMTP authentication.
    /// </summary>
    public string SmtpUserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for SMTP authentication.
    /// </summary>
    public string SmtpPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets who created these settings.
    /// </summary>
    public int CreatedBy { get; set; }
}
