namespace MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using MilkMatrix.Admin.Common.Extensions;
using MilkMatrix.Api.Common.Filters;
using MilkMatrix.Api.Common.Handlers;
using MilkMatrix.Api.Common.Helpers;
using MilkMatrix.Api.Models.AutomapperProfiles;
using MilkMatrix.Infrastructure.Models.AutomapperProfiles;
using MilkMatrix.Milk.Common.Extensions;

internal static class AppServicesConfigurationExtenstion
{
    public static IWebHostBuilder ConfigureServices(this WebApplicationBuilder builder) =>
    builder.ConfigureAppConfigurations()
        .ConfigureServices((hostContext, services) =>
        {
            services
              .AddAutoMapper(o =>
               {
                   o.AddProfile<AdminProfile>();
                   o.AddProfile<RolePagesPermissionsProfile>();
                   o.AddProfile<ModuleSubModuleMapping>();
                   o.AddProfile<GeographicalMappingProfile>();
                   o.AddProfile<NotificationProfileMapping>();
                   o.AddProfile<UploaderProfileMapping>();
                   o.AddProfile<PlantMappingProfile>();
                   o.AddProfile<BankMappingProfile>();
               })
              .AddMilkServicesDependencies(hostContext.Configuration)
              .AddConfiguration(hostContext.Configuration)
              .AddAdminServices()
              .ConfigureApiVersioning()
              .AddEndpointsApiExplorer()
              .ConfigureMvc()
              .AddCors(options =>
              {
                  options.AddPolicy(hostContext.HostingEnvironment.EnvironmentName, builder =>
                    {
                      builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                  });
              })
              .AddCustomCors(hostContext.Configuration)
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
