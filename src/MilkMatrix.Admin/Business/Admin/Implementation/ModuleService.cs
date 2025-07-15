using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Requests.Module;
using MilkMatrix.Admin.Models.Admin.Responses.Modules;
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

public class ModuleService : IModuleService
{
    private ILogging logger;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly IQueryMultipleData queryMultipleData;

    private readonly AppConfig appConfig;
    public ModuleService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig, IQueryMultipleData queryMultipleData)
    {
        this.logger = logger.ForContext("ServiceName", nameof(ModuleService));
        this.repositoryFactory = repositoryFactory;
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
        this.queryMultipleData = queryMultipleData ?? throw new ArgumentNullException(nameof(queryMultipleData), "QueryMultipleData cannot be null");
    }

    public async Task AddAsync(ModuleInsertRequest request)
    {
        try
        {
            logger.LogInfo($"AddAsync called for Module: {request.Name}");
            var repo = repositoryFactory.ConnectDapper<UserDetails>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["ModuleName"] = request.Name,
                ["OrderNumber"] = request.Order,
                ["ModuleIcon"] = request.Icon,
                ["CreatedBy"] = request.CreatedBy,
                ["Status"] = true,
                ["ActionType"] = request.ActionType
            };
            await repo.AddAsync(ModuleSpName.ModuleUpsert, parameters);
            logger.LogInfo($"Module {request.Name} added successfully.");
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
            logger.LogInfo($"DeleteAsync called for Module id: {id}");
            var repo = repositoryFactory.ConnectDapper<Module>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
        {
            {"ModuleId", id },
            {"Status", false },
            {"ModifyBy", userId },
            {"ActionType" , (int)CrudActionType.Delete }
        };
            await repo.DeleteAsync(ModuleSpName.ModuleUpsert, parameters);
            logger.LogInfo($"Module with id {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in DeleteAsync for Module id: {id}", ex);
            throw;
        }
    }

    public async Task<ModuleDetails?> GetByIdAsync(int id)
    {
        try
        {
            logger.LogInfo($"GetByIdAsync called for Module id: {id}");
            var repo = repositoryFactory
                       .ConnectDapper<Module>(DbConstants.Main);
            var data = await repo.QueryAsync<ModuleDetails>(ModuleSpName.GetModules, new Dictionary<string, object> { { "ModuleId", id },
                                                                            { "ActionType", (int)ReadActionType.Individual } }, null);

            var result = data.Any() ? data.FirstOrDefault() : default;
            logger.LogInfo(result != null
                ? $"Module with id {id} retrieved successfully."
                : $"Module with id {id} not found.");
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetByIdAsync for Module id: {id}", ex);
            throw;
        }
    }

    public async Task UpdateAsync(ModuleUpdateRequest request)
    {
        try
        {
            logger.LogInfo($"UpdateAsync called for user: {request.Name}");
            var repo = repositoryFactory.ConnectDapper<ModuleDetails>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["ModuleName"] = request.Name,
                ["ModuleId"] = request.Id,
                ["OrderNumber"] = request.Order,
                ["ModuleIcon"] = request.Icon,
                ["ModifyBy"] = request.ModifyBy,
                ["Status"] = request.IsActive,
                ["ActionType"] = request.ActionType
            };
            await repo.UpdateAsync(ModuleSpName.ModuleUpsert, parameters);
            logger.LogInfo($"Module {request.Name} updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in UpdateAsync for Module: {request.Name}", ex);
            throw;
        }
    }

    public async Task<IListsResponse<ModuleDetails>> GetAllAsync(IListsRequest request)
    {
        // 1. Fetch all results, count, and filter meta from stored procedure
        var (allResults, countResult, filterMetas) = await queryMultipleData
            .GetMultiDetailsAsync<ModuleDetails, int, FiltersMeta>(ModuleSpName.GetModules,
            DbConstants.Main,
            new Dictionary<string, object> { { "ActionType", (int)ReadActionType.All } },
            null);

        // 2. Build criteria from client request and filter meta
        var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters);
        var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
        var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

        // 3. Apply filtering, sorting, and paging
        var filtered = allResults.AsQueryable().ApplyFilters(filters);
        var sorted = filtered.ApplySorting(sorts);
        var paged = sorted.ApplyPaging(paging);

        // 4. Get count after filtering (before paging)
        var filteredCount = filtered.Count();

        // 5. Return result
        return new ListsResponse<ModuleDetails>
        {
            Count = filteredCount,
            Results = paged.ToList(),
            Filters = filterMetas
        };
    }
}
