namespace MilkMatrix.Infrastructure.Models.Config;

public sealed record DatabaseConfig
{
    /// <summary>
    ///     Settings section name.
    /// </summary>
    public const string SectionName = "Database";

    /// <summary>
    ///     The database connection string.
    /// </summary>
    public string RptConnectionString { get; init; } // we use init in the configuration files because the configured values shouldn't be changed once set.


    public string MainConnectionString { get; init; }

    /// <summary>
    /// Database commands timeout
    /// </summary>
    public int CommandTimeOut { get; set; } = 150;

    /// <summary>
    /// Database commands timeout
    /// </summary>
    public int BatchSize { get; set; } = 15000;

    /// <summary>
    /// Database commands timeout
    /// </summary>
    public int BulkCommandTimeOut { get; set; } = 600;
}
