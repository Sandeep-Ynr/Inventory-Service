using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Implementations;

namespace MilkMatrix.Milk.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMilkServicesDependencies
            (this IServiceCollection services, IConfiguration configuration)
        => services.AddScoped<IStateService, StateService>()
                   .AddScoped<IDistrictService, DistrictService>()
                   .AddScoped<ITehsilService, TehsilService>()
                   .AddScoped<IVillageService, VillageService>()
                   .AddScoped<IHamletService, HamletService>();
    }
}
