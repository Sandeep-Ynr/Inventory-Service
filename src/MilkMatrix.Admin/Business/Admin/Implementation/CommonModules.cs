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
    private readonly ILogging logger;
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

            return data != null && data.Any() ? data.DistinctBy(a => a.Id) : default;
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
                BusinessDetails = businessDetails?.DistinctBy(x => x.GetHashCode()),
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
        if (data != null && data.Any())
        {
            return data.FirstOrDefault();
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
            return Enumerable.Empty<Actions>();

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

    /// <summary>
    /// Fetches both submodule and page rows for the user, as returned by the new stored procedure.
    /// </summary>
    private async Task<List<PageList>> GetPageAndSubModuleListForUser(LoggedInUser userData, bool isSuperAdmin)
    {

        var allRows = new List<PageList>();
        var pages = Enumerable.Empty<PageList>();

        if (isSuperAdmin)
        {
            pages = await GetPageMenuLists("0", ReadActionType.All);
        }
        else
        {
            var roleIds = (userData.RoleId ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (var id in roleIds)
            {
                var rolePages = await GetPageMenuLists(id, ReadActionType.Individual);

                pages = pages.Concat(rolePages);
            }
        }

        allRows = pages.ToList();
        return allRows;
    }

    private async Task<IEnumerable<PageList>> GetPageMenuLists(string id, ReadActionType readActionType)
    {
        var repo = repositoryFactory.ConnectDapper<PageList>(DbConstants.Main);

        var requestParam = new Dictionary<string, object>
            {
                { "ActionType", (int)readActionType },
                { "Id", id }
            };

        var pages = await repo.QueryAsync<PageList>(
            AuthSpName.PageMenuList,
            requestParam,
            null,
            CommandType.StoredProcedure);
        return pages;
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
        var allRows = await GetPageAndSubModuleListForUser(userData, isSuperAdmin);

        // Attach actions to pages
        foreach (var page in allRows.Where(x => x.PageId != 0 && !string.IsNullOrEmpty(x.ActionId)))
        {
            page.ActionList = (await UserActionListAsync(userData.UserType, page.ActionId)).DistinctBy(a => a.Id).ToList();
        }

        response.ModuleList = BuildModuleTree(allRows);

        return response;
    }

    /// <summary>
    /// Builds the full module tree using lookup dictionaries for performance and robustness.
    /// </summary>
    private List<Module> BuildModuleTree(List<PageList> allRows)
    {
        // Build lookups for fast access
        var subModuleLookup = allRows
            .Where(x => x.SubModuleId != 0)
            .GroupBy(x => x.SubModuleId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var moduleLookup = allRows
            .Where(x => x.ModuleId != 0)
            .GroupBy(x => x.ModuleId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var modules = new List<Module>();

        foreach (var moduleGroup in moduleLookup)
        {
            AddModules(subModuleLookup, modules, moduleGroup);
        }

        return modules.OrderBy(m => m.OrderNumber).DistinctBy(m => m.Id).ToList();
    }

    private void AddModules(Dictionary<int, List<PageList>> subModuleLookup, List<Module> modules, KeyValuePair<int, List<PageList>> moduleGroup)
    {
        var moduleId = moduleGroup.Key;
        var moduleRows = moduleGroup.Value;
        var firstRow = moduleRows.First();

        var module = new Module
        {
            Id = moduleId,
            Name = firstRow.ModuleName,
            Icon = firstRow.ModuleIcon,
            OrderNumber = firstRow.ModuleOrderNumber,
            SubModuleList = BuildSubModuleTree(moduleRows, subModuleLookup, 0)
        };

        modules.Add(module);
    }

    /// <summary>
    /// Recursively builds the submodule tree using lookups for performance.
    /// </summary>
    private List<SubModule> BuildSubModuleTree(
        List<PageList> moduleRows,
        Dictionary<int, List<PageList>> subModuleLookup,
        int? parentSubModuleId = 0)
    {
        var subModules = moduleRows
            .Where(x => x.SubModuleId != 0 && (x.SubModuleParentId ?? 0) == (parentSubModuleId ?? 0))
            .GroupBy(x => x.SubModuleId)
            .Select(g => SelectSubModules(moduleRows, subModuleLookup, g))
            .Where(sm => sm != null)
            .OrderBy(sm => sm.OrderNumber)
            .DistinctBy(sm => sm.Id)
            .ToList();

        return subModules;
    }

    private SubModule? SelectSubModules(List<PageList> moduleRows, Dictionary<int, List<PageList>> subModuleLookup, IGrouping<int, PageList> g)
    {
        var subModuleId = g.Key;
        var baseRow = g.FirstOrDefault(x => x.PageId == 0) ?? g.First();

        // Defensive: log if parent is missing
        if (baseRow == null)
        {
            logger.LogWarning($"SubModuleId {subModuleId} has no base row.");
            return null;
        }

        // Recursively build children
        var children = BuildSubModuleTree(moduleRows, subModuleLookup, subModuleId);

        // Only include pages (PageId != 0) for this submodule
        var pagesForSubModule = g.Where(p => p.PageId != 0).ToList();
        var pageTree = BuildPageTree(pagesForSubModule, 0);

        // Log for diagnostics
        logger.LogInfo($"Building SubModule: {baseRow.SubModuleName} (Id: {subModuleId}), ParentId: {baseRow.SubModuleParentId}, Children: {children.Count}, Pages: {pageTree.Count}");

        return new SubModule
        {
            Id = subModuleId,
            Name = baseRow.SubModuleName,
            OrderNumber = baseRow.SubModuleOrderNumber,
            ParentId = baseRow.SubModuleParentId,
            Children = children,
            PageList = pageTree
        };
    }

    /// <summary>
    /// Recursively builds the page tree for a submodule.
    /// </summary>
    private List<PageList> BuildPageTree(List<PageList> pages, int? parentId = 0)
    {
        return pages
            .Where(p => (p.ParentId ?? 0) == (parentId ?? 0))
            .OrderBy(p => p.PageOrder)
            .Select(p =>
            {
                var children = BuildPageTree(pages, p.PageId);
                p.Children = children ?? new List<PageList>();
                return p;
            })
            .ToList();
    }
}
