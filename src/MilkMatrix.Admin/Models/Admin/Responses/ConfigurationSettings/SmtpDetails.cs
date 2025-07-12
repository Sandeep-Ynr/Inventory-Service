namespace MilkMatrix.Admin.Models.Admin.Responses.ConfigurationSettings;

/// <summary>
/// Represents the details of an SMTP configuration setting.
/// </summary>
public class SmtpDetails
{
    /// <summary>
    /// Gets or sets the unique identifier for the SMTP configuration setting.
    /// </summary>
    public int Id { get; set; }

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

    public bool IsActive { get; set; }
}
