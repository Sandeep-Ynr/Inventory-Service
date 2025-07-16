using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;
using static MilkMatrix.Milk.Models.Queries.GeographicalQueries;

namespace MilkMatrix.Milk.Implementations;

public class StateService : IStateService
{
    private readonly ILogging logging;
    private readonly AppConfig appConfig;
    private readonly IRepositoryFactory repositoryFactory;
    private readonly IQueryMultipleData queryMultipleData;

    public StateService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
    {
        this.logging = logging.ForContext("ServiceName", nameof(StateService));
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(StateService));
        this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        this.queryMultipleData = queryMultipleData;
    }

    public async Task<string> AddStateAsync(StateInsertRequest request)
    {
        try
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

            var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)CrudActionType.Create},
                { "StateName", request.StateName ?? (object)DBNull.Value },
                { "CountryId", request.CountryId ?? (object)DBNull.Value },
                { "AreaCode", request.AreaCode ?? (object)DBNull.Value },
                { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                { "CreatedBy", request.CreatedBy }
            };

            var response = await repository.QueryAsync<CommonLists>(
                StateQueries.AddStates, requestParams, null, CommandType.StoredProcedure
            );

            return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return "Error occurred";
        }
    }

    public async Task<string> UpdateStateAsync(StateUpdateRequest request)
    {
        try
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

            var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)CrudActionType.Update },
                { "StateId", request.StateId},
                { "StateName", request.StateName ?? (object)DBNull.Value },
                { "CountryId", request.CountryId ?? (object)DBNull.Value },
                { "AreaCode", request.AreaCode ?? (object)DBNull.Value },
                { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                { "ModifyBy", request.ModifyBy }
            };

            var response = await repository.QueryAsync<CommonLists>(
                StateQueries.AddStates, requestParams, null, CommandType.StoredProcedure
            );

            return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return "Error occurred";
        }
    }
    public async Task<string> DeleteAsync(int id, int userId)
    {
        try
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"StateId", id },
                {"IsStatus", false },
                {"ModifyBy", userId },
                {"ActionType" , (int)CrudActionType.Delete }
            };

            var response = await repository.QueryAsync<CommonLists>(
                StateQueries.AddStates, requestParams, null, CommandType.StoredProcedure
            );

            return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return "Error occurred";
        }

    }
    public async Task<StateResponse?> GetByIdAsync(int id)
    {
        try
        {
            var repo = repositoryFactory
                       .ConnectDapper<StateResponse>(DbConstants.Main);
            var data = await repo.QueryAsync<StateResponse>(StateQueries.GetStatesList, new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "StateId", id }
                }, null);

            var result = data.Any() ? data.FirstOrDefault() : new StateResponse();
            logging.LogInfo(result != null
                ? $"State with id {id} retrieved successfully."
                : $"State with id {id} not found.");
            return result;
        }
        catch (Exception ex)
        {
            logging.LogError($"Error in GetByIdAsync for State id: {id}", ex);
            throw;
        }
    }

    public async Task<IEnumerable<CommonLists>> GetSpecificLists(StateRequest request)
    {
        var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
        var requestParams = new Dictionary<string, object>
        {
            { "ActionType", (int)request.ActionType },
            { "StateId", request.StateId },
            { "CountryId", request.CountryId },
            { "IsStatus", request.IsActive }
        };
        var response = await repository.QueryAsync<CommonLists>(StateQueries.GetStates, requestParams, null, CommandType.StoredProcedure);

        return response;
    }
    public async Task<IEnumerable<StateResponse>> GetStates(StateRequest request)
    {
        var repository = repositoryFactory.Connect<StateResponse>(DbConstants.Main);
        var requestParams = new Dictionary<string, object>
        {
            { "ActionType", (int)request.ActionType },
            { "StateId", request.StateId },
            { "CountryId", request.CountryId },
            { "IsStatus", request.IsActive }
        };
        var response = await repository.QueryAsync<StateResponse>(StateQueries.GetStates, requestParams, null, CommandType.StoredProcedure);
       
        return response;
    }
    public async Task<IListsResponse<StateResponse>> GetAllAsync(IListsRequest request)
    {
        var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

        // 1. Fetch all results, count, and filter meta from stored procedure
        var (allResults, countResult, filterMetas) = await queryMultipleData
            .GetMultiDetailsAsync<StateResponse, int, FiltersMeta>(StateQueries.GetStatesList,
                DbConstants.Main,
                parameters,
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
        return new ListsResponse<StateResponse>
        {
            Count = filteredCount,
            Results = paged.ToList(),
            Filters = filterMetas
        };
    }

}
