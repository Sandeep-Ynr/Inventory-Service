using Asp.Versioning;

namespace MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions
{
    public static class ApiVersioningConfigurationExtensions
    {
        public static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(
              config =>
              {
                  config.ReportApiVersions = true;
                  config.AssumeDefaultVersionWhenUnspecified = true;
                  config.DefaultApiVersion = new ApiVersion(1, 0);
              })
               .AddApiExplorer(
               options =>
               {
                   options.GroupNameFormat = "'v'VVV";

                   // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                   // can also be used to control the format of the API version in route templates
                   options.SubstituteApiVersionInUrl = true;
               });
            // format the version as "'v'major"
            return services;
        }
    }
}
