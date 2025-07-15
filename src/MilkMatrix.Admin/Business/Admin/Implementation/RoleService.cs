using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Requests.Role;
using MilkMatrix.Admin.Models.Admin.Responses.Role;
using MilkMatrix.Admin.Models.Admin.Responses.User;
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

public class RoleService : IRoleService
{
    private ILogging logger;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly IQueryMultipleData queryMultipleData;

    private readonly AppConfig appConfig;
    public RoleService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig, IQueryMultipleData queryMultipleData)
    {
        this.logger = logger.ForContext("ServiceName", nameof(RoleService));
        this.repositoryFactory = repositoryFactory;
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
        this.queryMultipleData = queryMultipleData ?? throw new ArgumentNullException(nameof(queryMultipleData), "QueryMultipleData cannot be null");
    }

    public async Task AddAsync(RoleInsertRequest request)
    {
        try
        {
            logger.LogInfo($"AddAsync called for role: {request.RoleName}");
            var repo = repositoryFactory.ConnectDapper<UserDetails>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["RoleName"] = request.RoleName,
                ["BusinessId"] = request.BusinessId,
                ["CreatedBy"] = request.CreatedBy,
                ["Status"] = true,
                ["ActionType"] = (int)CrudActionType.Create
            };
            await repo.AddAsync(RoleSpName.RoleUpsert, parameters);
            logger.LogInfo($"Role {request.RoleName} added successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in AddAsync for user: {request.RoleName}", ex);
            throw;
        }
    }

    public async Task DeleteAsync(int id, int userId)
    {
        try
        {
            logger.LogInfo($"DeleteAsync called for role id: {id}");
            var repo = repositoryFactory.ConnectDapper<Roles>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                {"RoleID", id },
                {"Status", false },
                {"ModifyBy", userId },
                {"ActionType" , (int)CrudActionType.Delete }
            };
            await repo.DeleteAsync(RoleSpName.RoleUpsert, parameters);
            logger.LogInfo($"Role with id {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in DeleteAsync for role id: {id}", ex);
            throw;
        }
    }

    public async Task<RoleDetails?> GetByIdAsync(int id)
    {
        try
        {
            logger.LogInfo($"GetByIdAsync called for role id: {id}");
            var repo = repositoryFactory
                       .ConnectDapper<RoleDetails>(DbConstants.Main);
            var data = await repo.QueryAsync<RoleDetails>(RoleSpName.GetRoles, new Dictionary<string, object> { { "RoleId", id },
                                                                                { "ActionType", (int)ReadActionType.Individual } }, null);

            var result = data.Any() ? data.FirstOrDefault() : default;
            logger.LogInfo(result != null
                ? $"Role with id {id} retrieved successfully."
                : $"Role with id {id} not found.");
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetByIdAsync for user id: {id}", ex);
            throw;
        }
    }

    public async Task UpdateAsync(RoleUpdateRequest request)
    {
        try
        {
            logger.LogInfo($"UpdateAsync called for user: {request.RoleName}");
            var repo = repositoryFactory.ConnectDapper<UserDetails>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["RoleID"] = request.RoleId,
                ["RoleName"] = request.RoleName,
                ["BusinessId"] = request.BusinessId,
                ["ModifyBy"] = request.ModifyBy,
                ["Status"] = request.IsActive,
                ["ActionType"] = (int)CrudActionType.Update
            };
            await repo.UpdateAsync(RoleSpName.RoleUpsert, parameters);
            logger.LogInfo($"Role {request.RoleName} updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in UpdateAsync for role: {request.RoleName}", ex);
            throw;
        }
    }

    public async Task<IListsResponse<RoleDetails>> GetAllAsync(IListsRequest request)
    {
        // 1. Fetch all results, count, and filter meta from stored procedure
        var (allResults, countResult, filterMetas) = await queryMultipleData
            .GetMultiDetailsAsync<RoleDetails, int, FiltersMeta>(RoleSpName.GetRoles,
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
        return new ListsResponse<RoleDetails>
        {
            Count = filteredCount,
            Results = paged.ToList(),
            Filters = filterMetas
        };
    }
}
