using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MilkMatrix.Logging.Contracts;
using MilkMatrix.Logging.Implementation;
using Serilog;

namespace MilkMatrix.Logging.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>  
    /// Configures Serilog for the host, with optional file logging.  
    /// </summary>  
    /// <param name="builder">The IHostBuilder (e.g., from Program.cs)</param>  
    /// <param name="logFilePath">Optional file path for log output</param>  
    /// <returns>The IHostBuilder for chaining</returns>  
    public static IHostBuilder AddLogging(this IHostBuilder builder, string? logFilePath = null)
    {
        builder.UseSerilog((context, loggerConfiguration) =>
             context.Configuration.ConfigureInfraLogging(loggerConfiguration, logFilePath)
        );
        return builder;
    }

    public static IServiceCollection RegisterLoggingDependencies(this IServiceCollection services) => services.AddSingleton<ILogs>(_ => new Logs(Log.Logger));
}
