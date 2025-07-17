using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MilkMatrix.Logging.Config;
using Serilog;
using Serilog.Enrichers.Sensitive;
using Serilog.Exceptions;

namespace MilkMatrix.Logging.Extensions;

public static class LoggingExtensions
{
    public static ILogger ConfigureLogger(this IConfiguration configuration, string filePath = "")
    {
        var loggerConfig = configuration.GetSection(LoggerConfig.SectionName).Get<LoggerConfig>();

        var logBasePath = string.IsNullOrWhiteSpace(filePath)
              ? loggerConfig?.DefaultLogPath ?? "logs"
              : filePath;

        var logger = new LoggerConfiguration()
            .ReadFrom
            .Configuration(configuration)
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
                    path: $"{logBasePath}/{serviceName}/log.json",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true
                )
            )
            .CreateLogger();

        return logger;
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
