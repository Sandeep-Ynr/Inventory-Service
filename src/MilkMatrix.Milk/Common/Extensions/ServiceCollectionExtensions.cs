using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MilkMatrix.Milk.Contracts.Animal;
using MilkMatrix.Milk.Contracts.Bank;
using MilkMatrix.Milk.Contracts.Bmc;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Contracts.Logistics.Route;
using MilkMatrix.Milk.Contracts.Logistics.Transporter;
using MilkMatrix.Milk.Contracts.Logistics.Vehicle;
using MilkMatrix.Milk.Contracts.Logistics.Vendor;
using MilkMatrix.Milk.Contracts.Mcc;
using MilkMatrix.Milk.Contracts.Member;
using MilkMatrix.Milk.Contracts.Member.MemberAddress;
using MilkMatrix.Milk.Contracts.Member.MemberBankDetails;
using MilkMatrix.Milk.Contracts.Member.MemberDocuments;
using MilkMatrix.Milk.Contracts.Member.MilkProfile;
using MilkMatrix.Milk.Contracts.Milk;
using MilkMatrix.Milk.Contracts.MPP;
using MilkMatrix.Milk.Contracts.Party;
using MilkMatrix.Milk.Contracts.Plant;
using MilkMatrix.Milk.Contracts.SahayakVSP;
using MilkMatrix.Milk.Implementations;
using MilkMatrix.Milk.Implementations.Animal;
using MilkMatrix.Milk.Implementations.Bmc;
using MilkMatrix.Milk.Implementations.Logistics.Route;
using MilkMatrix.Milk.Implementations.Logistics.VehicleType;
using MilkMatrix.Milk.Implementations.Mcc;
using MilkMatrix.Milk.Implementations.Member.Address;
using MilkMatrix.Milk.Implementations.Member.MemberBankDetails;
using MilkMatrix.Milk.Implementations.Member.MemberDocuments;
using MilkMatrix.Milk.Implementations.Member.MilkProfile;
using MilkMatrix.Milk.Implementations.Milk;
using MilkMatrix.Milk.Implementations.Plant;

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
                    .AddScoped<IAnimalService, AnimalService>()
                    .AddScoped<IPartyGroupService, PartyGroupService>()
                    .AddScoped<IPartyService, PartyService>()
                    .AddScoped<IMilkService, MilkService>()
                    .AddScoped<ITransporterService, TransporterService>()
                    .AddScoped<IVehicleService, VehicleService>()
                    .AddScoped<IVendorService, VendorService>()
                    .AddScoped<IMemberService, MemberService>()
                    .AddScoped<IMemberAddressService, MemberAddressService>()
                    .AddScoped<IMemberDocumentsService, MemberDocumentsService>()
                    .AddScoped<IMemberMilkProfileService, MemberMilkProfileService>()
                    .AddScoped<IMemberBankDetailsService, MemberBankDetailsService>();
    }
}
