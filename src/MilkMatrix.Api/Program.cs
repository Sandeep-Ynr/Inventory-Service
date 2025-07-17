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

            builder.Services
                .AddConfigs(builder.Configuration);

            builder.Host.AddInfra();

            Log.Information("Application startup: Logging configuration applied.");

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
}
