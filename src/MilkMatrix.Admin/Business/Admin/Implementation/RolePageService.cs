using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Requests.RolePage;
using MilkMatrix.Admin.Models.Admin.Responses.Page;
using MilkMatrix.Admin.Models.Admin.Responses.RolePage;
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

public class RolePageService : IRolePageService
{
    private ILogging logger;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly IQueryMultipleData queryMultipleData;

    private readonly AppConfig appConfig;
    public RolePageService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig, IQueryMultipleData queryMultipleData)
    {
        this.logger = logger.ForContext("ServiceName", nameof(RolePageService));
        this.repositoryFactory = repositoryFactory;
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
        this.queryMultipleData = queryMultipleData ?? throw new ArgumentNullException(nameof(queryMultipleData), "QueryMultipleData cannot be null");
    }

    public async Task AddAsync(RolePageInsertRequest request)
    {
        try
        {
            logger.LogInfo($"AddAsync called for page: {request.RoleId}, {request.PageId}");
            var repo = repositoryFactory.ConnectDapper<RolePageInsertRequest>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["RoleId"] = request.RoleId,
                ["PageId"] = request.PageId,
                ["BusinessId"] = request.BusinessId,
                ["ActionID"] = request.ActionDetails,
                ["ActionType"] = (int)CrudActionType.Create,
                ["CreatedBy"] = request.CreatedBy,
            };
            await repo.AddAsync(RolePageSpName.RolePageUpsert, parameters);
            logger.LogInfo($"Page {request.RoleId}, {request.PageId} added successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in AddAsync for page: {request.RoleId}, {request.PageId}", ex);
            throw;
        }
    }

    public async Task DeleteAsync(int id, int businessId, int userId)
    {
        try
        {
            logger.LogInfo($"DeleteAsync called for role id: {id}");
            var repo = repositoryFactory.ConnectDapper<RolePages>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["RoleId"] = id,
                ["BusinessId"] = businessId,
                ["ActionType"] = (int)CrudActionType.Delete,
                ["ModifyBy"] = userId,
            };
            await repo.DeleteAsync(RolePageSpName.RolePageUpsert, parameters);
            logger.LogInfo($"RolePage with id {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in DeleteAsync for page id: {id}", ex);
            throw;
        }
    }

    public async Task<IEnumerable<RolePages>> GetByIdAsync(int roleId, int businessId)
    {
        try
        {
            logger.LogInfo($"GetByIdAsync called for page id: {roleId}, {businessId}");
            var repo = repositoryFactory
                       .ConnectDapper<PageList>(DbConstants.Main);
            var data = await repo.QueryAsync<RolePages>(RolePageSpName.GetRolePages,
                new Dictionary<string, object> {
                    { "RoleId", roleId },
                { "BusinessId", businessId },
                { "ActionType", (int)ReadActionType.Individual} }, null);

            var result = data.Any() ? data : Enumerable.Empty<RolePages>();
            logger.LogInfo(result != null
                ? $"Page with id {roleId}, {businessId} retrieved successfully."
                : $"Page with id {roleId} , {businessId} not found.");
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetByIdAsync for page id: {roleId}, {businessId}", ex);
            throw;
        }
    }

    public async Task UpdateAsync(RolePageUpdateRequest request)
    {
        try
        {
            logger.LogInfo($"UpdateAsync called for page: {request.RoleId}, {request.PageId}");
            var repo = repositoryFactory.ConnectDapper<RolePageUpdateRequest>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["RoleId"] = request.RoleId,
                ["PageId"] = request.PageId,
                ["BusinessId"] = request.BusinessId,
                ["ActionID"] = request.ActionDetails,
                ["ActionType"] = (int)CrudActionType.Update,
                ["ModifyBy"] = request.ModifyBy,
            };
            await repo.UpdateAsync(RolePageSpName.RolePageUpsert, parameters);
            logger.LogInfo($"Role Page {request.RoleId} ,  {request.PageId} updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in UpdateAsync for role page: {request.RoleId} , {request.PageId}", ex);
            throw;
        }
    }

    public async Task<IListsResponse<RolePages>> GetAllAsync(IListsRequest request)
    {
        // 1. Fetch all results, count, and filter meta from stored procedure
        var (allResults, countResult, filterMetas) = await queryMultipleData
            .GetMultiDetailsAsync<RolePages, int, FiltersMeta>(RolePageSpName.GetRolePages,
            DbConstants.Main,
            new Dictionary<string, object> { { "ActionType", (int)ReadActionType.All } },
            null);

        // 2. Build criteria from client request and filter meta
        var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Search);
        var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
        var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

        // 3. Apply filtering, sorting, and paging
        var filtered = allResults.AsQueryable().ApplyFilters(filters);
        var sorted = filtered.ApplySorting(sorts);
        var paged = sorted.ApplyPaging(paging);

        // 4. Get count after filtering (before paging)
        var filteredCount = filtered.Count();

        // 5. Return result
        return new ListsResponse<RolePages>
        {
            Count = filteredCount,
            Results = paged.ToList(),
            Filters = filterMetas
        };
    }
}
