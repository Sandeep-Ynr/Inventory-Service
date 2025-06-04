namespace MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;

public static class CacheConfigurationExtension
{
    public static IServiceCollection AddCache(this IServiceCollection services) =>
        services.AddMemoryCache();
}
