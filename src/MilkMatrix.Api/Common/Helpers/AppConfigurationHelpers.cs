namespace MilkMatrix.Api.Common.Helpers;

using MilkMatrix.Api.Common.Middleware;

public static class AppConfigurationHelpers
{
    public static IApplicationBuilder AddCustomErrorMiddleware(this IApplicationBuilder builder) =>
      builder
       .UseMiddleware<CustomErrorHandler>();

    public static WebApplicationBuilder ConfigureAppConfigurations(this WebApplicationBuilder builder)
    {
        builder.Configuration.ConfigureConfigurationBuilder(builder.Environment);
        return builder;
    }

    public static void ConfigureConfigurationBuilder(this IConfigurationBuilder config, IWebHostEnvironment environmentName) =>
        config
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{environmentName.EnvironmentName.ToLower()}.json",
                true)
            .AddEnvironmentVariables();

    public static string GetEnvironment() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
#if DEBUG
                                             Environments.Development;
#else
                                                Environments.Production;
#endif
}
