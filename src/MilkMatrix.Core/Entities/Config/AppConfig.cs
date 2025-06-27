namespace MilkMatrix.Core.Entities.Config;

/// <summary>
/// Application Configurations
/// </summary>
public sealed record AppConfig
{
    /// <summary>
    /// Configuration Section Name to fetch
    /// </summary>
    public const string SectionName = "AppConfiguration";

    /// <summary>
    /// Encription Key
    /// </summary>
    public string Base64EncryptKey { get; set; }

    /// <summary>
    /// Host Name 
    /// </summary>
    public string HostName { get; set; }

    /// <summary>
    /// AllowToSendMail
    /// </summary>
    public bool AllowToSendMail { get; set; }

    /// <summary>
    /// AllowToCreateOTP 
    /// </summary>
    public bool AllowToCreateOTP { get; set; }

    /// <summary>
    /// MerchantPrefix
    /// </summary>
    public string MerchantPrefix { get; set; }

    /// <summary>
    /// Http client name used for user agent
    /// </summary>
    public string ClientName { get; set; } = string.Empty;

    /// <summary>
    /// AllowToStoreOrdPwd 
    /// </summary>
    public bool AllowToStoreOrgPwd { get; set; }
}
