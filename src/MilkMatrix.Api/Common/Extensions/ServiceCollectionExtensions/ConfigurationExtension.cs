namespace MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;

/// <summary>
/// This file is used to setup all configuration objects meant to be injected as IOptions
/// </summary>
public static class ConfigurationExtension
{
    /// Add configuration from different sections of app settings
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration) => 
        services.AddOptions();
}
