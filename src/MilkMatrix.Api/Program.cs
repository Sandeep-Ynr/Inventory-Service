using MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;
using MilkMatrix.Api.Common.Helpers;
using MilkMatrix.Infrastructure.Extensions;
using Serilog;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.AddInfraLogging(builder.Environment.ContentRootPath);

            builder.Services.AddLoggingServices();
            builder.ConfigureAppConfigurations();
            builder.Services.AddConfigs(builder.Configuration);
            builder.Host.AddInfra();
            builder.Services.ConfigureServices(builder.Configuration, builder.Environment);

            var appBuilt = builder.Build();
            var app = appBuilt.ConfigureApp();
            Log.Information("Application startup: Logging configuration applied.");
            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while starting the application: " + ex.Message);
        }

        finally
        {
            Log.CloseAndFlush();
        }
    }
}
