using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilkMatrix.Milk.Contracts.Bank;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Contracts.Mcc;
using MilkMatrix.Milk.Contracts.MPP;
using MilkMatrix.Milk.Contracts.Plant;
using MilkMatrix.Milk.Contracts.Route;
using MilkMatrix.Milk.Contracts.SahayakVSP;
using MilkMatrix.Milk.Contracts.Vehicle;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Implementations.Mcc;
using MilkMatrix.Milk.Implementations.Plant;
using MilkMatrix.Milk.Contracts.Bmc;
using MilkMatrix.Milk.Implementations.Bmc;
using MilkMatrix.Milk.Contracts.Animal;
using MilkMatrix.Milk.Implementations.Animal;

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
                   .AddScoped<IHamletService, HamletService>()
                    .AddScoped<IBankRegService, BankRegService>()
                    .AddScoped<IBankTypeService, BankTypeService>()
                    .AddScoped<IBankService, BankService>()
                    .AddScoped<IBranchService, BranchService>()
                    .AddScoped<IPlantService, PlantService>()
                    .AddScoped<IRouteService, RouteService>()
                    .AddScoped<IVehicleTypeService, VehicleTypeService>()
                    .AddScoped<ISahayakVSPService, SahayakVSPService>()
                    .AddScoped<IMccService, MccService>()
                    .AddScoped<IMPPService, MPPService>()
                    .AddScoped<IBmcService, BmcService>()
                    .AddScoped<IAnimalService, AnimalService>();
    }
}
