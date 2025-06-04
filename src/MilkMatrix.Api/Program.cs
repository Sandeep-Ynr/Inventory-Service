//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast =  Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast");

//app.Run();

//record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}

using MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;
using MilkMatrix.Api.Common.Helpers;
using Serilog;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        CreateStaticLogger();
        try
        {
            Log.Logger.Information("Application Started.");

            var builder = WebApplication.CreateBuilder(args);
            builder.ConfigureServices();

            await builder
                .ConfigureApp()
                .RunAsync();
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "Application crash!");
        }
        finally
        {
            Log.CloseAndFlush();
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
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(cfg.Build()).CreateLogger();
    }
}
