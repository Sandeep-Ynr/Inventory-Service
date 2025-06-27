using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Requests.SubModule;
using MilkMatrix.Admin.Models.Admin.Responses.SubModules;
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

public class SubModuleService : ISubModuleService
{
    private ILogging logger;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly IQueryMultipleData queryMultipleData;

    private readonly AppConfig appConfig;
    public SubModuleService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig, IQueryMultipleData queryMultipleData)
    {
        this.logger = logger.ForContext("ServiceName", nameof(SubModuleService));
        this.repositoryFactory = repositoryFactory;
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
        this.queryMultipleData = queryMultipleData ?? throw new ArgumentNullException(nameof(queryMultipleData), "QueryMultipleData cannot be null");
    }

    public async Task AddAsync(SubModuleInsertRequest request)
    {
        try
        {
            logger.LogInfo($"AddAsync called for subModule: {request.Name}");
            var repo = repositoryFactory.ConnectDapper<UserDetails>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["SubModuleName"] = request.Name,
                ["OrderNumber"] = request.Order,
                ["CreatedBy"] = request.CreatedBy,
                ["Status"] = true,
                ["ActionType"] = request.ActionType
            };
            await repo.AddAsync(SubModuleSpName.SubModuleUpsert, parameters);
            logger.LogInfo($"subModule {request.Name} added successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in AddAsync for user: {request.Name}", ex);
            throw;
        }
    }

    public async Task DeleteAsync(int id, int userId)
    {
        try
        {
            logger.LogInfo($"DeleteAsync called for subModule id: {id}");
            var repo = repositoryFactory.ConnectDapper<SubModuleDetails>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
        {
            {"SubModuleId", id },
            {"Status", false },
            {"ModifyBy", userId },
            {"ActionType" , (int)CrudActionType.Delete }
        };
            await repo.DeleteAsync(SubModuleSpName.SubModuleUpsert, parameters);
            logger.LogInfo($"subModule with id {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in DeleteAsync for subModule id: {id}", ex);
            throw;
        }
    }

    public async Task<SubModuleDetails?> GetByIdAsync(int id)
    {
        try
        {
            logger.LogInfo($"GetByIdAsync called for subModule id: {id}");
            var repo = repositoryFactory
                       .ConnectDapper<SubModuleDetails>(DbConstants.Main);
            var data = await repo.QueryAsync<SubModuleDetails>(SubModuleSpName.GetSubModules, new Dictionary<string, object> { { "SubModuleId", id },
                                                                            { "ActionType", (int)ReadActionType.Individual } }, null);

            var result = data.Any() ? data.FirstOrDefault() : default;
            logger.LogInfo(result != null
                ? $"subModule with id {id} retrieved successfully."
                : $"subModule with id {id} not found.");
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetByIdAsync for subModule id: {id}", ex);
            throw;
        }
    }

    public async Task UpdateAsync(SubModuleUpdateRequest request)
    {
        try
        {
            logger.LogInfo($"UpdateAsync called for user: {request.Name}");
            var repo = repositoryFactory.ConnectDapper<SubModuleDetails>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["SubModuleName"] = request.Name,
                ["SubModuleId"] = request.Id,
                ["OrderNumber"] = request.Order,
                ["ModifyBy"] = request.ModifyBy,
                ["Status"] = request.IsAcive,
                ["ActionType"] = request.ActionType
            };
            await repo.UpdateAsync(SubModuleSpName.SubModuleUpsert, parameters);
            logger.LogInfo($"subModule {request.Name} updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in UpdateAsync for subModule: {request.Name}", ex);
            throw;
        }
    }

    public async Task<IListsResponse<SubModuleDetails>> GetAllAsync(IListsRequest request)
    {
        // 1. Fetch all results, count, and filter meta from stored procedure
        var (allResults, countResult, filterMetas) = await queryMultipleData
            .GetMultiDetailsAsync<SubModuleDetails, int, FiltersMeta>(SubModuleSpName.GetSubModules,
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
        return new ListsResponse<SubModuleDetails>
        {
            Count = filteredCount,
            Results = paged.ToList(),
            Filters = filterMetas
        };
    }
}
