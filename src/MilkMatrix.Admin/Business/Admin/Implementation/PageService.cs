using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin;
using MilkMatrix.Admin.Models.Admin.Requests.Page;
using MilkMatrix.Admin.Models.Admin.Responses.Page;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using static MilkMatrix.Admin.Models.Constants;

namespace MilkMatrix.Admin.Business.Admin.Implementation;

public class PageService : IPageService
{
    private ILogging logger;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly IQueryMultipleData queryMultipleData;

    private readonly AppConfig appConfig;
    public PageService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig, IQueryMultipleData multipleData)
    {
        this.logger = logger.ForContext("ServiceName", nameof(PageService));
        this.repositoryFactory = repositoryFactory;
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
        this.queryMultipleData = multipleData ?? throw new ArgumentNullException(nameof(multipleData), "QueryMultipleData cannot be null");
    }

    public async Task AddAsync(PageInsertRequest request)
    {
        try
        {
            logger.LogInfo($"AddAsync called for page: {request.PageName}");
            var repo = repositoryFactory.ConnectDapper<PageInsertRequest>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["PageName"] = request.PageName,
                ["PageUrl"] = request.PageUrl,
                ["ModuleId"] = request.ModuleId,
                ["SubModuleId"] = request.SubModuleId,
                ["PageOrder"] = request.PageOrder,
                ["IsMenu"] = request.IsMenu,
                ["PageIcon"] = request.PageIcon,
                ["ActionDetails"] = request.ActionDetails,
                ["Status"] = true,
                ["ActionType"] = (int)CrudActionType.Create,
                ["CreatedBy"] = request.CreatedBy,
            };
            await repo.AddAsync(PageSpName.PageUpsert, parameters);
            logger.LogInfo($"Page {request.PageName} added successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in AddAsync for page: {request.PageName}", ex);
            throw;
        }
    }

    public async Task DeleteAsync(int id, int userId)
    {
        try
        {
            logger.LogInfo($"DeleteAsync called for role id: {id}");
            var repo = repositoryFactory.ConnectDapper<PageList>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["PageId"] = id,
                ["Status"] = false,
                ["ActionType"] = (int)CrudActionType.Delete,
                ["ModifyBy"] = userId,
            };
            await repo.DeleteAsync(PageSpName.PageUpsert, parameters);
            logger.LogInfo($"Page with id {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in DeleteAsync for page id: {id}", ex);
            throw;
        }
    }

    public async Task<Pages?> GetByIdAsync(int id)
    {
        try
        {
            logger.LogInfo($"GetByIdAsync called for page id: {id}");
            var repo = repositoryFactory
                       .ConnectDapper<Pages>(DbConstants.Main);
            var data = await repo.QueryAsync<Pages>(PageSpName.GetPages, new Dictionary<string, object> { { "PageId", id },
                                                                            { "ActionType", (int)ReadActionType.Individual } }, null);

            var result = data.Any() ? data.FirstOrDefault() : default;
            logger.LogInfo(result != null
                ? $"Page with id {id} retrieved successfully."
                : $"Page with id {id} not found.");
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetByIdAsync for page id: {id}", ex);
            throw;
        }
    }

    public async Task UpdateAsync(PageUpdateRequest request)
    {
        try
        {
            logger.LogInfo($"UpdateAsync called for page: {request.PageName}");
            var repo = repositoryFactory.ConnectDapper<PageUpdateRequest>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["PageId"] = request.PageId,
                ["PageName"] = request.PageName,
                ["PageUrl"] = request.PageUrl,
                ["ModuleId"] = request.ModuleId,
                ["SubModuleId"] = request.SubModuleId,
                ["PageOrder"] = request.PageOrder,
                ["IsMenu"] = request.IsMenu,
                ["PageIcon"] = request.PageIcon,
                ["ActionDetails"] = request.ActionDetails,
                ["Status"] = true,
                ["ActionType"] = (int)CrudActionType.Update,
                ["ModifyBy"] = request.ModifyBy,
            };
            await repo.UpdateAsync(PageSpName.PageUpsert, parameters);
            logger.LogInfo($"Page {request.PageName} updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in UpdateAsync for page: {request.PageName}", ex);
            throw;
        }
    }

    public async Task<IListsResponse<Pages>> GetAllAsync(IListsRequest request)
    {
        // 1. Fetch all results, count, and filter meta from stored procedure
        var (allResults, countResult, filterMetas) = await queryMultipleData
            .GetMultiDetailsAsync<Pages, int, FiltersMeta>(PageSpName.GetPages,
            DbConstants.Main,
            new Dictionary<string, object> { { "ActionType", (int)ReadActionType.All } },
            null);

        // 2. Build criteria from client request and filter meta
        var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
        var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
        var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

        // 3. Apply filtering, sorting, and paging
        var filtered = allResults.AsQueryable().ApplyFilters(filters);
        var sorted = filtered.ApplySorting(sorts);
        var paged = sorted.ApplyPaging(paging);

        // 4. Get count after filtering (before paging)
        var filteredCount = filtered.Count();

        // 5. Return result
        return new ListsResponse<Pages>
        {
            Count = filteredCount,
            Results = paged.ToList(),
            Filters = filterMetas
        };
    }

    public async Task<IEnumerable<CommonLists>?> GetPagesForApprovalAsync(int? id = null)
    {
        try
        {
            var repo = repositoryFactory
                           .ConnectDapper<CommonLists>(DbConstants.Main);

            var requestParams = new Dictionary<string, object>
            {
                { "BusinessId", id },
                { "ActionType", id != null && id > 0 ? ReadActionType.Individual : ReadActionType.All }
            };
            var data = await repo.QueryAsync<CommonLists>(PageSpName.GetPagesForApproval, requestParams, null);

            return data != null && data.Count() > 0 ? data : default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            return default;
        }
    }
}
