using System.Data;
using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin;
using MilkMatrix.Admin.Models.Admin.Common;
using MilkMatrix.Admin.Models.Admin.Requests.Business;
using MilkMatrix.Admin.Models.Admin.Responses.Modules;
using MilkMatrix.Admin.Models.Admin.Responses.Page;
using MilkMatrix.Admin.Models.Admin.Responses.Role;
using MilkMatrix.Admin.Models.Admin.Responses.SubModules;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response.Business;
using static MilkMatrix.Admin.Models.Constants;

namespace MilkMatrix.Admin.Business.Admin.Implementation;

public class CommonModules : ICommonModules
{
    private ILogging logger;
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IQueryMultipleData queryMultipleData;
    private readonly AppConfig appConfig;

    public CommonModules(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig, IQueryMultipleData queryMultipleData)
    {
        this.repositoryFactory = repositoryFactory;
        this.logger = logger.ForContext("ServiceName", nameof(CommonModules));
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
        this.queryMultipleData = queryMultipleData;
    }

    public async Task<CommonUserDetails> GetCommonDetails(string userId, string mobileNumber)
    {
        try
        {
            return await GetCommonData(await GetUserData(userId, mobileNumber));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            return default;
        }
    }

    public async Task<ModuleResponse> GetModulesAsync(string userId, string mobileNumber)
    {
        var response = new ModuleResponse();
        var userData = await GetUserData(userId, mobileNumber);
        if (userData == null)
        {
            logger.LogError("User data not found.", null);
            return response;
        }

        bool isSuperAdmin = userData.UserType == 0;
        var flatPages = await GetPageListForUser(userData, isSuperAdmin);

        // Group actions for each page before building the tree
        var flatPagesWithGroupedActions = GroupActionsForPages(flatPages);

        // Normalize parent IDs for robust tree building
        foreach (var page in flatPagesWithGroupedActions)
        {
            page.ParentId = page.ParentId ?? 0;
            page.SubModuleParentId = page.SubModuleParentId ?? 0;
        }

        // Build n-level module/submodule/page tree
        response.ModuleList = BuildModuleList(flatPagesWithGroupedActions);

        return response;
    }

    public async Task<IEnumerable<FinancialYearDetails>> GetFinancialYearAsync(FinancialYearRequest request)
    {
        try
        {
            if (request == null)
            {
                logger.LogError("FinancialYearRequest is null.");
                return Enumerable.Empty<FinancialYearDetails>();
            }

            var requestParams = new Dictionary<string, object>
            {
                { "Id", request.Id },
                { "ActionType", request.ActionType },
                { "Status", request.IsActive }
            };

            var repo = repositoryFactory.ConnectDapper<FinancialYearDetails>(DbConstants.Main);
            var result = await repo.QueryAsync<FinancialYearDetails>(
                AuthSpName.GetFinancialYear,
                requestParams,
                null,
                CommandType.StoredProcedure);

            if (result == null || !result.Any())
            {
                logger.LogWarning("No financial year details found for the given request.");
                return Enumerable.Empty<FinancialYearDetails>();
            }
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            return Enumerable.Empty<FinancialYearDetails>();
        }
    }

    public async Task<IEnumerable<Actions>?> GetActionDetailsAsync(int? id = null)
    {
        try
        {
            var repo = repositoryFactory
                           .ConnectDapper<Actions>(DbConstants.Main);

            var requestParams = new Dictionary<string, object>
            {
                { "ActionId", id },
                { "ActionType", id != null && id > 0 ? ReadActionType.Individual : ReadActionType.All }
            };
            var data = await repo.QueryAsync<Actions>(AuthSpName.GetActions, requestParams, null);

            return data != null && data.Count() > 0 ? data.DistinctBy(a => a.Id) : default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            return default;
        }
    }

    private async Task<CommonUserDetails> GetCommonData(LoggedInUser? userData)
    {
        var commonList = new CommonUserDetails();

        if (userData != null)
        {
            var requestParam = new Dictionary<string, object>
            {
                { "StatusType", "User" },
                { "RoleIds", userData.UserType != 0 ? userData.RoleId : "" },
                { "BusinessId", userData.BusinessId },
                { "UserType", userData.UserType },
                { "UserId", userData.UserId }
            };

            var (businessDetails, roles, reportingDetails, userTypes, siteDetails, financialYearDetails) =
                await queryMultipleData.GetMultiDetailsAsync<BusinessData, Roles, ReportingDetails, CommonProps, SiteDetails, FinancialYearDetails>(
                    AuthSpName.GetCommonDetails, DbConstants.Main, requestParam, null);

            commonList = new CommonUserDetails
            {
                BusinessDetails = businessDetails?.DistinctBy(x => x.GetHashCode()), // or use a key property
                Roles = roles?.DistinctBy(x => x.Id),
                ReportingDetails = reportingDetails?.DistinctBy(x => x.GetHashCode()),
                UserTypes = userTypes?.DistinctBy(x => x.Id),
                SiteDetails = siteDetails?.DistinctBy(x => x.GetHashCode()),
                FinancialYearDetails = financialYearDetails?.DistinctBy(x => x.Id)
            };
        }
        return commonList;
    }

    private async Task<LoggedInUser?> GetUserData(string userId, string mobileNumber, int businessId = 0)
    {
        var requestParam = new Dictionary<string, object> { { "UserId", userId } };
        var repo = repositoryFactory
                       .ConnectDapper<LoggedInUser>(DbConstants.Main);
        var data = await repo.QueryAsync<LoggedInUser>(AuthSpName.LoginUserDetails, requestParam, null);
        var user = new LoggedInUser();
        if (data != null && data.Count() > 0)
        {
            user = data.FirstOrDefault();
            return user;
        }
        else
        {
            logger.LogError("GetUserData", new Exception("Some error occurred while retrieving details for logged in user."));
            return default;
        }
    }

    private async Task<IEnumerable<Actions>> UserActionListAsync(int userType, string actionIdList)
    {
        if (string.IsNullOrWhiteSpace(actionIdList))
            return new List<Actions>();

        var actionIds = actionIdList.Split(',').ToList();

        var repo = repositoryFactory.ConnectDapper<Actions>(DbConstants.Main);

        var requestParam = new Dictionary<string, object>
        {
            { "UserType", userType },
            { "ID", string.Join(",", actionIds) }
        };

        var actions = await repo.QueryAsync<Actions>(
            AuthSpName.ActionList,
            requestParam,
            null,
            CommandType.StoredProcedure);

        return actions?.DistinctBy(a => a.Id) ?? Enumerable.Empty<Actions>();
    }

    private async Task<List<PageList>> GetPageListForUser(LoggedInUser userData, bool isSuperAdmin)
    {
        var repo = repositoryFactory.ConnectDapper<PageList>(DbConstants.Main);
        var pList = new List<PageList>();
        var pages = Enumerable.Empty<PageList>();

        if (isSuperAdmin)
        {
            var requestParam = new Dictionary<string, object>
            {
                { "ActionType", 2 },
                { "Id", 0 }
            };

            pages = await repo.QueryAsync<PageList>(
                AuthSpName.PageMenuList,
                requestParam,
                null,
                CommandType.StoredProcedure);
        }
        else
        {
            var roleIds = (userData.RoleId ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var id in roleIds)
            {
                var requestParam = new Dictionary<string, object>
                {
                    { "ActionType", 1 },
                    { "Id", id }
                };

                var rolePages = await repo.QueryAsync<PageList>(
                    AuthSpName.PageMenuList,
                    requestParam,
                    null,
                    CommandType.StoredProcedure);

                pages = pages.Concat(rolePages);
            }
        }

        foreach (var page in pages)
        {
            var actionIdList = string.IsNullOrEmpty(page.ActionId) ? "" : page.ActionId;
            page.ActionList = await UserActionListAsync(userData.UserType, actionIdList);
            pList.Add(page);
        }

        // Distinct by PageId
        return pList.DistinctBy(x => x.PageId).ToList();
    }

    // Group actions for each page before building the menu tree
    private List<PageList> GroupActionsForPages(IEnumerable<PageList> flatPages)
    {
        var result = new List<PageList>();
        foreach (var pageGroup in flatPages.GroupBy(p => p.PageId))
        {
            var first = pageGroup.First();
            var allActions = pageGroup
                .SelectMany(p => p.ActionList ?? Enumerable.Empty<Actions>())
                .GroupBy(a => a.Id)
                .Select(g => g.First())
                .ToList();

            result.Add(new PageList
            {
                PageId = first.PageId,
                PageName = first.PageName,
                PageOrder = first.PageOrder,
                PageURL = first.PageURL,
                PageIcon = first.PageIcon,
                ModuleId = first.ModuleId,
                ModuleName = first.ModuleName,
                ModuleIcon = first.ModuleIcon,
                ModuleOrderNumber = first.ModuleOrderNumber,
                SubModuleId = first.SubModuleId,
                SubModuleName = first.SubModuleName,
                SubModuleOrderNumber = first.SubModuleOrderNumber,
                ActionId = first.ActionId,
                RoleId = first.RoleId,
                ParentId = first.ParentId,
                SubModuleParentId = first.SubModuleParentId,
                Children = first.Children,
                ActionList = allActions.DistinctBy(a => a.Id).ToList()
            });
        }
        // Distinct by PageId
        return result.DistinctBy(x => x.PageId).ToList();
    }

    // n-layer menu tree builder
    private IEnumerable<Module> BuildModuleList(IEnumerable<PageList> flatList)
    {
        // Group by ModuleId for root modules
        var modules = flatList
            .GroupBy(x => x.ModuleId)
            .Select(g => new Module
            {
                Id = g.Key,
                Name = g.First().ModuleName,
                Icon = g.First().ModuleIcon,
                OrderNumber = g.First().ModuleOrderNumber,
                SubModuleList = BuildSubModuleTree(flatList.Where(x => x.ModuleId == g.Key).ToList(), 0)
            })
            .OrderBy(m => m.OrderNumber)
            .DistinctBy(m => m.Id)
            .ToList();

        return modules;
    }

    // Recursively build n-level submodule tree
    private List<SubModule> BuildSubModuleTree(List<PageList> flatList, int? parentSubModuleId = 0)
    {
        var subModules = flatList
            .Where(x => x.SubModuleId != 0 && (x.SubModuleParentId ?? 0) == (parentSubModuleId ?? 0))
            .GroupBy(x => x.SubModuleId)
            .Select(g => new SubModule
            {
                Id = g.Key,
                Name = g.First().SubModuleName,
                OrderNumber = g.First().SubModuleOrderNumber,
                ParentId = g.First().SubModuleParentId,
                Children = BuildSubModuleTree(flatList, g.Key),
                PageList = BuildPageTree(flatList.Where(p => p.SubModuleId == g.Key).ToList(), 0)
            })
            .OrderBy(sm => sm.OrderNumber)
            .DistinctBy(sm => sm.Id)
            .ToList();

        return subModules;
    }

    // Recursively build n-level page tree
    private List<PageList> BuildPageTree(List<PageList> flatList, int? parentPageId = 0)
    {
        var pages = flatList
            .Where(x => (x.ParentId ?? 0) == (parentPageId ?? 0))
            .OrderBy(x => x.PageOrder)
            .Select(x =>
            {
                var node = new PageList
                {
                    PageId = x.PageId,
                    PageName = x.PageName,
                    PageURL = x.PageURL,
                    PageIcon = x.PageIcon,
                    PageOrder = x.PageOrder,
                    ModuleId = x.ModuleId,
                    ModuleName = x.ModuleName,
                    ModuleIcon = x.ModuleIcon,
                    ModuleOrderNumber = x.ModuleOrderNumber,
                    SubModuleId = x.SubModuleId,
                    SubModuleName = x.SubModuleName,
                    SubModuleOrderNumber = x.SubModuleOrderNumber,
                    ActionId = x.ActionId,
                    RoleId = x.RoleId,
                    ActionList = x.ActionList?.DistinctBy(a => a.Id).ToList(),
                    ParentId = x.ParentId,
                    SubModuleParentId = x.SubModuleParentId,
                    Children = BuildPageTree(flatList, x.PageId)
                };
                return node;
            })
            .DistinctBy(x => x.PageId)
            .ToList();

        return pages;
    }

    // Optional: flatten tree if needed for UI
    private List<PageList> FlattenMenuTree(IEnumerable<PageList> tree)
    {
        var result = new List<PageList>();
        foreach (var node in tree)
        {
            result.Add(node);
            if (node.Children != null && node.Children.Count > 0)
                result.AddRange(FlattenMenuTree(node.Children));
        }
        return result.DistinctBy(x => x.PageId).ToList();
    }
}
