using Microsoft.Extensions.DependencyInjection;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Business.Admin.Implementation;
using MilkMatrix.Admin.Business.Auth.Contracts;
using MilkMatrix.Admin.Business.Auth.Contracts.Service;
using MilkMatrix.Admin.Business.Auth.Services;
using MilkMatrix.Admin.Common.Handlers.Approval;
using MilkMatrix.Core.Abstractions.Approval.Handler;

namespace MilkMatrix.Admin.Common.Extensions;

public static class ServiceCollectionExtensions
{
    // This method can be used to register services in the IServiceCollection
    public static IServiceCollection AddAdminServices(this IServiceCollection services) =>
        services.AddScoped<IAuth, Auth>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IRoleService, RoleService>()
            .AddScoped<IPageService, PageService>()
            .AddScoped<IRolePageService, RolePageService>()
            .AddScoped<IModuleService, ModuleService>()
            .AddScoped<ISubModuleService, SubModuleService>()
            .AddScoped<IBusinessService, BusinessService>()
            .AddScoped<IConfigurationService, ConfigurationService>()
            .AddScoped<ICommonModules, CommonModules>()
            .AddScoped<ITokenProcess, TokenProcess>()
            .AddScoped<IReportService, ReportingService>()
            .AddTransient<IApprovalHandler, StatusPageHandler>();
}
