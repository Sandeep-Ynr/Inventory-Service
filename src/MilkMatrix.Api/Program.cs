using MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;
using MilkMatrix.Api.Common.Helpers;
using MilkMatrix.Infrastructure.Extensions;
using Serilog;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        CreateStaticLogger();
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddInfra();
            
            builder.ConfigureServices();
            
            await builder
                .ConfigureApp()
                .RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while starting the application: " + ex.Message);
        }
    }

    /// <summary>
    ///     Creates the default static logger with the configuration provided.
    /// </summary>
    private static void CreateStaticLogger()
    {
        // We need to build twice the configuration to use it for the static logger.
        var cfg = new ConfigurationBuilder();
        cfg.ConfigureConfigurationBuilder(AppConfigurationHelpers.GetEnvironment());
    }
}
