using Microsoft.Extensions.DependencyInjection;

namespace MilkMatrix.Core.Extensions;

public static class ServiceCollectionExtensions
{
    // This method can be used to register services in the IServiceCollection
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // Register your services here
        // Example:
        //services.AddScoped<IUserService, UserService>();
        //services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
        return services;
    }
}
