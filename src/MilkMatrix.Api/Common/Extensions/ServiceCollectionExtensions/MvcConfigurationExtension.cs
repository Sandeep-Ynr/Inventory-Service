namespace MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;

using Microsoft.AspNetCore.Mvc;
using MilkMatrix.Domain.Entities.Responses;
using System.Net;
using static MilkMatrix.Api.Common.Constants.Constants;

public static class MvcConfigurationExtension
{
    public static IServiceCollection ConfigureMvc(this IServiceCollection services)
    {
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var validationError = context.ModelState
                        .Select(x => x.Value.Errors)
                        .FirstOrDefault(y => y.Count > 0)?.FirstOrDefault();

                    return new BadRequestObjectResult(new ErrorResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        ErrorMessage = validationError?.ErrorMessage
                    })
                    {
                        ContentTypes =
                        {
                            AppConstants.ErrorContentType
                        }
                    };
                };
            });

        return services;
    }
}
