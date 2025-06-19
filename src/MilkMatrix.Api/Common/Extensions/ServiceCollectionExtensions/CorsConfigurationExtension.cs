using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static MilkMatrix.Api.Common.Constants.Constants;

public static class CorsConfigurationExtension
{
    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        // Read the comma-separated origins from configuration
        var origins = configuration.GetSection("AppConfiguration:HostName").Value?
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        services.AddCors(options =>
        {
            options.AddPolicy(AppConstants.AllowCredentials, builder =>
            {
                builder.WithOrigins(origins ?? Array.Empty<string>())
                       .AllowCredentials()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });

            options.AddPolicy(AppConstants.AllowAllOrigin, policy =>
            {
                policy.WithOrigins(origins ?? Array.Empty<string>())
                      .AllowCredentials()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }
}
