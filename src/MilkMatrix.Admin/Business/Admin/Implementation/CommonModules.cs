using System.Data;
using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin;
using MilkMatrix.Admin.Models.Admin.Common;
using MilkMatrix.Admin.Models.Admin.Responses.Page;
using MilkMatrix.Admin.Models.Admin.Responses.Role;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Infrastructure.Models.Config;
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
        var pList = await GetPageListForUser(userData, isSuperAdmin);

        var mList = BuildModuleList(pList, isSuperAdmin);
        response.ModuleList = mList;
        response.PageList = ExtractAllPages(mList);

        return response;
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


            var (businessDetails, roles, reportingDetails, userTypes, siteDetails) = await queryMultipleData.GetMultiDetailsAsync<BusinessData, Roles, ReportingDetails, CommonProps, SiteDetails>(AuthSpName.GetCommonDetails, DbConstants.Main, requestParam, null);

            reportingDetails.ForEach(item =>
                {
                    if (!string.IsNullOrEmpty(item.EmailId))
                        item.EmailId = appConfig.Base64EncryptKey.EncryptString(item.EmailId);
                });
            commonList = new CommonUserDetails
            {
                BusinessDetails = businessDetails,
                Roles = roles,
                ReportingDetails = reportingDetails,
                UserTypes = userTypes,
                SiteDetails = siteDetails
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

        return await repo.QueryAsync<Actions>(
            AuthSpName.ActionList, // Use your actual stored procedure name
            requestParam,
            null,
            CommandType.StoredProcedure);
    }

    private IEnumerable<SubModule> GetSubModuleList(IEnumerable<IGrouping<int, PageList>> groupbySubModuleId, bool isUserSuperAdmin)
    {
        List<SubModule> sList = new List<SubModule>();
        int subModelid = 0; string subModelName = string.Empty;
        foreach (var list in groupbySubModuleId)
        {
            foreach (PageList m in list)
            {
                subModelid = m.SubModuleId;
                subModelName = m.SubModuleName;
                break;
            }
            var groupbyPageId = list.GroupBy(s => s.PageId);
            sList.Add(
                new SubModule
                {
                    Id = subModelid,
                    Name = subModelName,
                    PageList = GetPageList(groupbyPageId, isUserSuperAdmin)
                });
        }
        return sList;
    }//retriving user sub-module list
    private IEnumerable<PageList> GetPageList(IEnumerable<IGrouping<int, PageList>> groupbyPageId, bool isUserSuperAdmin)
    {
        List<PageList> pList = new List<PageList>();
        int pageid = 0; string pageName = string.Empty;
        int pageOrder = 0; string pageURL = string.Empty; string pageIcon = string.Empty;
        foreach (var list in groupbyPageId)
        {
            List<Actions> aList = new List<Actions>();
            pageid = 0;
            foreach (PageList m in list)
            {
                if (pageid == 0)
                {
                    pageid = m.PageId;
                    pageName = m.PageName;
                    pageOrder = m.PageOrder;
                    pageURL = m.PageURL;
                    pageIcon = m.PageIcon;
                }
                foreach (var a in m.ActionList)
                {
                    aList.Add(
                        new Actions
                        {
                            Id = a.Id,
                            Name = a.Name
                        });
                }
            }
            var groupbyActionId = aList.GroupBy(s => s.Id);
            pList.Add(
                new PageList
                {
                    PageId = pageid,
                    PageName = pageName,
                    PageOrder = pageOrder,
                    PageURL = pageURL,
                    PageIcon = pageIcon,
                    ActionList = GetActionList(groupbyActionId)
                });
        }
        return pList;
    }//retriving user page list
    private IEnumerable<Actions> GetActionList(IEnumerable<IGrouping<int, Actions>> groupbyActionId)
    {
        List<Actions> aList = new List<Actions>();
        int actionid = 0; string actionName = string.Empty;
        foreach (var list in groupbyActionId)
        {
            foreach (Actions a in list)
            {
                actionid = a.Id;
                actionName = a.Name;
                break;
            }
            aList.Add(
                new Actions
                {
                    Id = actionid,
                    Name = actionName
                });
        }
        return aList;
    }

    private async Task<IEnumerable<PageList>> GetPageListForUser(LoggedInUser userData, bool isSuperAdmin)
    {
        var pList = new List<PageList>();
        var repo = repositoryFactory.ConnectDapper<PageList>(DbConstants.Main);

        if (!isSuperAdmin)
        {
            var roleList = !string.IsNullOrWhiteSpace(userData.RoleId)
                ? userData.RoleId.Split(',').ToList()
                : new List<string>();

            foreach (var id in roleList)
            {
                var requestParam = new Dictionary<string, object>
                                            {
                                                { "ActionType", 1 },
                                                { "Id", id }
                                            };

                var pages = await repo.QueryAsync<PageList>(
                    AuthSpName.PageMenuList, // Use your actual stored procedure name
                    requestParam,
                    null,
                    CommandType.StoredProcedure);

                foreach (var page in pages)
                {
                    page.ActionList = await UserActionListAsync(userData.UserType, string.IsNullOrEmpty(page.ActionId) ? "" : page.ActionId);
                }
                pList.AddRange(pages);
            }
        }
        else
        {
            var requestParam = new Dictionary<string, object>
                                            {
                                                { "ActionType", 2 },
                                                { "Id", 0 }
                                            };

            var pages = await repo.QueryAsync<PageList>(
                AuthSpName.PageMenuList, // Use your actual stored procedure name
                requestParam,
                null,
                CommandType.StoredProcedure);

            foreach (var page in pages)
            {
                page.ActionList = await UserActionListAsync(userData.UserType, string.IsNullOrEmpty(page.ActionId) ? "" : page.ActionId);
            }
            pList.AddRange(pages);
        }
        return pList;
    }

    private IEnumerable<Module> BuildModuleList(IEnumerable<PageList> pList, bool isSuperAdmin)
    {
        var mList = new List<Module>();
        var groupbyModuleId = pList.OrderBy(s => s.SubModuleOrderNumber).GroupBy(s => s.ModuleId);

        foreach (var moduleGroup in groupbyModuleId)
        {
            var first = moduleGroup.First();
            var groupbySubModuleId = moduleGroup.OrderBy(s => s.SubModuleOrderNumber).GroupBy(s => s.SubModuleId);
            mList.Add(new Module
            {
                Id = first.ModuleId,
                Name = first.ModuleName,
                Icon = first.ModuleIcon,
                SubModuleList = GetSubModuleList(groupbySubModuleId, isSuperAdmin)
            });
        }
        return mList;
    }

    private IEnumerable<PageList> ExtractAllPages(IEnumerable<Module> mList)
    {
        var pgList = new List<PageList>();
        foreach (var subModule in from module in mList
                                  from subModule in module.SubModuleList
                                  select subModule)
        {
            pgList.AddRange(subModule.PageList!.Select(p => new PageList
            {
                PageId = p.PageId,
                ActionList = p.ActionList,
                ModuleId = p.ModuleId,
                ModuleName = p.ModuleName,
                PageIcon = p.PageIcon,
                PageName = p.PageName,
                PageOrder = p.PageOrder,
                PageURL = p.PageURL,
                RoleId = p.RoleId,
                SubModuleId = p.SubModuleId,
                SubModuleName = p.SubModuleName,
                ActionId = p.ActionId
            }));
        }

        return pgList;
    }

}
