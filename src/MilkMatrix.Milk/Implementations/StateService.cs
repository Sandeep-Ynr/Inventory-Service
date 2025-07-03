using System.Data;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response;
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

    public StateService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory)
    {
        this.logging = logging.ForContext("ServiceName", nameof(StateService));
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(StateService));
        this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
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
}
