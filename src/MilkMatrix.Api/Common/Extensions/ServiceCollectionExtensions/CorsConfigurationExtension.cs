using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;

public static class CorsConfigurationExtension
{
    public static IServiceCollection AddCors(this IServiceCollection services, string environmentName)
    {
        services.AddCors(options =>
        {
            switch (environmentName)
            {
                case AppConstants.DevEnvironment:
                case AppConstants.SandboxEnvironment:
                    {
                        options.AddDefaultPolicy(builder =>
                        {
                            builder.SetIsOriginAllowed(origin => new Uri(origin).Host == AppConstants.Localhost);
                            builder.AllowAnyMethod();
                            builder.AllowAnyHeader();
                            builder.AllowCredentials();
                        });
                        break;
                    }

            }

            options.AddPolicy(AppConstants.AllowAllOrigin,
                builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });

            options.AddPolicy(AppConstants.AllowCredentials,
                builder =>
                {
                    builder.AllowCredentials();
                });
        });
        return services;
    }
}
