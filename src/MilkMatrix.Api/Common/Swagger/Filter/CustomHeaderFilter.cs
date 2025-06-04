namespace MilkMatrix.Api.Common.Swagger.Filter;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class CustomHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        var apiDescription = context.ApiDescription;

        if (apiDescription.IsDeprecated())
        {
            operation.Deprecated = true;
        }
    }
}
