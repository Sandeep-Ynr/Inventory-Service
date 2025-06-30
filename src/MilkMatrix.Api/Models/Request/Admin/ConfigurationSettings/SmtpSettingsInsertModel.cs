using System.ComponentModel.DataAnnotations;

namespace MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings;

/// <summary>
/// Represents a request to insert new SMTP settings into the system.
/// </summary>
public class SmtpSettingsInsertModel
{
    [Required]
    /// <summary>
    /// Gets or sets the SMTP server address.
    /// </summary>
    public string SmtpServer { get; set; } = string.Empty;

    [Required]
    /// <summary>
    /// Gets or sets the SMTP port number.
    /// </summary>
    public int SmtpPort { get; set; }

    [Required]
    /// <summary>
    /// Gets or sets the username for SMTP authentication.
    /// </summary>
    public string SmtpUserId { get; set; } = string.Empty;

    [Required]
    /// <summary>
    /// Gets or sets the password for SMTP authentication.
    /// </summary>
    public string SmtpPassword { get; set; } = string.Empty;
}
