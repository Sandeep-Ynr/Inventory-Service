using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;
using static MilkMatrix.Api.Common.Constants.Constants;

namespace MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;

public static class SwaggerConfigurationExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services) => SwaggerOptionsHelpers.ConfigureSwaggerOptions(services);

    public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, IWebHostEnvironment hostingEnvironment) => app.UseSwagger().UseSwaggerUI(c =>
    {
        c.DocumentTitle = SwaggerConstants.ApiDocumentationTitle;
        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json", hostingEnvironment.EnvironmentName + " " + description.GroupName.ToLowerInvariant());
        }
        c.InjectStylesheet(SwaggerConstants.CssPath);
        c.DocExpansion(DocExpansion.None);
    });
}
