namespace MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;

using MilkMatrix.Api.Common.Filters;
using MilkMatrix.Api.Common.Handlers;
using MilkMatrix.Api.Common.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Extensions;
using MilkMatrix.Admin.Common.Extensions;

internal static class AppServicesConfigurationExtenstion
{
    public static IWebHostBuilder ConfigureServices(this WebApplicationBuilder builder) =>
    builder.ConfigureAppConfigurations()
        .ConfigureServices((hostContext, services) =>
        {
            services.AddConfiguration(hostContext.Configuration)
            .AddCoreServices()
            .AddConfigs(hostContext.Configuration)
            .ConfigureInfraservices(hostContext.Configuration)
            .AddAdminServices()
            .ConfigureApiVersioning()
            .AddEndpointsApiExplorer()
            .ConfigureMvc()
            .AddCors(hostContext.HostingEnvironment.EnvironmentName)
            .AddCache()
            .AddSwagger(hostContext.Configuration)
            .AddHttpClient()
            .AddHttpContextAccessor()
            .Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 268435456; });
            // Add the healthChecks
            services.AddHealthChecks();

            services.AddAuthentication("custom").AddScheme<AuthenticationSchemeOptions, CustomTokenHandler>("custom", opt => { });
            // custom action filter
            services.AddScoped<ModelValidationAttribute>();
        });
}
