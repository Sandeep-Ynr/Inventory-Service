namespace MilkMatrix.Api.Common.Helpers;

using MilkMatrix.Api.Common.Middleware;

public static class AppConfigurationHelpers
{      
    public static IApplicationBuilder AddCustomErrorMiddleware(this IApplicationBuilder builder) =>
      builder
       .UseMiddleware<CustomErrorHandler>();

    public static IWebHostBuilder ConfigureAppConfigurations(this WebApplicationBuilder builder) => builder.WebHost
        .ConfigureAppConfiguration((hostingContext, config) =>
            config.ConfigureConfigurationBuilder(hostingContext.HostingEnvironment.EnvironmentName)
        );

    public static void ConfigureConfigurationBuilder(this IConfigurationBuilder config, string environmentName) =>
        config
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{environmentName.ToLower()}.json",
                true)
            .AddEnvironmentVariables();
    //.ReplaceTokens();

    public static string GetEnvironment() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
#if DEBUG
                                             Environments.Development;
#else
                                            Environments.Production;
#endif
}
