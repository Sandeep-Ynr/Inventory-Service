namespace MilkMatrix.Logging.Config;
public class LoggerConfig
{
    public const string SectionName = "LoggerConfiguration";
    public string? DefaultLogPath { get; set; }

    public string? UploadServiceLogPath { get; set; }

    public string? BaseLogPath { get; set; }
}
