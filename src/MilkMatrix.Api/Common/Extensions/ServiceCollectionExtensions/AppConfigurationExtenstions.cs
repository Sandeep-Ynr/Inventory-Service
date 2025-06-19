namespace MilkMatrix.Api.Common.Extensions.ServiceCollectionExtensions;

using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.HttpOverrides;
using MilkMatrix.Api.Common.Helpers;
using static MilkMatrix.Api.Common.Constants.Constants;

public static class AppConfigurationExtenstions
{
    public const string AllowAllOrigin = "AllowAllOrigins";

    public static WebApplication ConfigureApp(this WebApplicationBuilder builder)
    {
       
        var app = builder.Build();
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        _ = app.Environment.IsDevelopment()
      ? app.UseDeveloperExceptionPage()
      : app.UseHsts();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(provider, app.Environment);
            app.UseSwaggerUI();
        }

        app
            .AddCustomErrorMiddleware()
            .UseHttpsRedirection()
            .UseRouting()
            .UseCors(AppConstants.AllowAllOrigin)
            .UseAuthentication()
            .UseAuthorization()
 
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            })
            .UseHealthChecks("/health");
      
        return app;
    }
}
