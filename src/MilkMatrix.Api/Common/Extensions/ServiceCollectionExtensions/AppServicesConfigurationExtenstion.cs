namespace MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using MilkMatrix.Admin.Common.Extensions;
using MilkMatrix.Api.Common.Filters;
using MilkMatrix.Api.Common.Handlers;
using MilkMatrix.Api.Models.AutomapperProfiles;
using MilkMatrix.Infrastructure.Models.AutomapperProfiles;
using MilkMatrix.Milk.Common.Extensions;

internal static class AppServicesConfigurationExtenstion
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
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
                o.AddProfile<SahayakVSPMappingProfile>();
                o.AddProfile<MccMappingProfile>();
                o.AddProfile<MPPMappingProfile>();
                o.AddProfile<BmcMappingProfile>();
                o.AddProfile<AnimalMappingProfile>();
                o.AddProfile<PartyMappingProfile>();
                o.AddProfile<MilkMappingProfile>();
                o.AddProfile<LogisticsMappingProfile>();
                o.AddProfile<MemberMappingProfile>();
            })
            .AddMilkServicesDependencies(configuration)
            .AddConfiguration(configuration)
            .AddAdminServices()
            .ConfigureApiVersioning()
            .AddEndpointsApiExplorer()
            .ConfigureMvc()
            .AddCors(options =>
            {
                options.AddPolicy(environment.EnvironmentName, builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            })
            .AddCustomCors(configuration)
            .AddCache()
            .AddSwagger()
            .AddHttpClient()
            .AddHttpContextAccessor()
            .Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 268435456; });

        services.AddHealthChecks();
        services.AddAuthentication("custom").AddScheme<AuthenticationSchemeOptions, CustomTokenHandler>("custom", opt => { });
        services.AddScoped<ModelValidationAttribute>();

        return services;
    }
}
