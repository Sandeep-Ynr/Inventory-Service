using Asp.Versioning.ApiExplorer;
using MilkMatrix.Api.Common.Swagger.Filter;
using Microsoft.OpenApi.Models;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;

internal static class SwaggerOptionsHelpers
{
    public static IServiceCollection ConfigureSwaggerOptions(this IServiceCollection services)
    {
        // Register SwaggerGen
        services.AddSwaggerGen(swaggerOptions =>
        {
            swaggerOptions.EnableAnnotations();

            // -- we can un-commit these once we have a complete template for us
            // c.ExampleFilters();
            // c.IncludeXmlComments(xmlPath);
            swaggerOptions.AddSecurityDefinition(AppConstants.Bearer, new OpenApiSecurityScheme
            {
                Description = SwaggerConstants.AuthenticationDescription,
                Name = SwaggerConstants.Authorization,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            swaggerOptions.CustomSchemaIds(type => type.ToString());
            swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
            swaggerOptions.OperationFilter<CustomHeaderFilter>();
        });

        // Register a post-configuration action to add Swagger docs for each API version
        services.PostConfigure<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>(options =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        });

        return services;
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "MilkMatrix",
            Version = description.ApiVersion.ToString(),
            Description = "MilkMatrix"
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}
