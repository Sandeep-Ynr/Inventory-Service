using Microsoft.Extensions.Configuration;
using MilkMatrix.Logging.Config;
using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Exceptions;
using Serilog.Formatting.Json;

namespace MilkMatrix.Logging.Extensions;

public static class LoggingExtensions
{
    public static void ConfigureInfraLogging(this IConfiguration configuration, LoggerConfiguration loggerConfiguration, string filePath = "")
    {
        var loggerConfig = configuration.GetSection(LoggerConfig.SectionName).Get<LoggerConfig>();

        // Ensure logBasePath is not null or empty
        var logBasePath = string.IsNullOrWhiteSpace(filePath)
            ? loggerConfig?.BaseLogPath
            : filePath;

        if (string.IsNullOrWhiteSpace(logBasePath))
            throw new InvalidOperationException("Log base path is not configured.");

        var defaultLogPath = loggerConfig?.DefaultLogPath;
        if (string.IsNullOrWhiteSpace(defaultLogPath))
            throw new InvalidOperationException("Default log path is not configured.");

        var logDirectory = Path.Combine(logBasePath, defaultLogPath);

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .ConfigureLogMasking(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console()
            .WriteTo.Map(
                keyPropertyName: "ServiceName",
                defaultKey: "General",
                 configure: (serviceName, wt) => wt.File(
                            path: $"{logDirectory}/{serviceName}/log.json",
                            rollingInterval: RollingInterval.Day,
                            rollOnFileSizeLimit: true,
                            formatter: new JsonFormatter())
                        );
    }

    private static LoggerConfiguration ConfigureLogMasking(this LoggerConfiguration self, IConfiguration configuration)
    {
        var maskingCfg = configuration
            .GetSection(SerilogLogMaskingConfig.SectionName)
            .Get<SerilogLogMaskingConfig>();

        if (maskingCfg?.Disabled == false)
        {
            self.Enrich.WithSensitiveDataMasking(
                options =>
                {
                    if (maskingCfg.MaskText != null)
                    {
                        options.MaskValue = maskingCfg.MaskText;
                    }

                    if (maskingCfg.MaskProperties?.Any() == true)
                    {
                        options.MaskProperties.AddRange(maskingCfg.MaskProperties);
                    }

                    // To avoid any default behaviour, we keep control of what will be used.
                    options.MaskingOperators.Clear();
                    if (maskingCfg.EnableEmailAddressMaskingOperator)
                    {
                        options.MaskingOperators.Add(new EmailAddressMaskingOperator());
                    }
                    if (maskingCfg.RegexPatternMaskingOperators?.Any() == true)
                    {
                        options.MaskingOperators.AddRange(maskingCfg.RegexPatternMaskingOperators.Select(opCfg => new RegexPatternMaskingOperator(opCfg)));
                    }
                });
        }

        return self;
    }

}
