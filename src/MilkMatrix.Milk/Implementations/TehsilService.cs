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

namespace MilkMatrix.Milk.Implementations
{
    public class TehsilService : ITehsilService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;


        public TehsilService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory)
        {
            this.logging = logging.ForContext("ServiceName", nameof(TehsilService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));

        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(TehsilRequest request)
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"TehsilId", request.TehsilId},
                {"DistrictId", request.DistrictId},
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<CommonLists>(TehsilQueries.GetTehsil, requestParams, null, CommandType.StoredProcedure);

            return response;

        }

        public async Task<IEnumerable<TehsilResponse>> GetTehsils(TehsilRequest request)
        {
            var repository = repositoryFactory.Connect<TehsilResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"TehsilId", request.TehsilId},
                {"DistrictId", request.DistrictId},
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<TehsilResponse>(TehsilQueries.GetTehsil, requestParams, null, CommandType.StoredProcedure);

            return response;
        }

        public async Task<TehsilResponse?> GetByIdAsync(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Tehsil id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<TehsilResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<TehsilResponse>(TehsilQueries.GetTehsil, new Dictionary<string, object> 
                { 
                    { "ActionType", 2 },
                    { "TehsilId", id },
                    { "IsStatus", true }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new TehsilResponse();
                logging.LogInfo(result != null
                    ? $"Tehsil with id {id} retrieved successfully."
                    : $"Tehsil with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Tehsil id: {id}", ex);
                throw;
            }
        }
        public async Task AddTehsilAsync(TehsilInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create},
                    { "TehsilName", request.TehsilName ?? (object)DBNull.Value },
                    { "DistrictId", request.DistrictId ?? (object)DBNull.Value },
                    { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy }
                    
                };

                var response = await repository.AddAsync(TehsilQueries.AddTehsil, requestParams, CommandType.StoredProcedure
                );

                logging.LogInfo($"Tehsil {request.TehsilName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Tehsil: {request.TehsilName}", ex);
                throw;
            }
        }

        public async Task UpdateTehsilAsync(TehsilUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "TehsilId", request.TehsilId},
                    { "TehsilName", request.TehsilName ?? (object)DBNull.Value },
                    { "DistrictId", request.DistrictId},
                    { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                var response = await repository.UpdateAsync(
                   TehsilQueries.AddTehsil, requestParams, CommandType.StoredProcedure
                );

                logging.LogInfo($"Tehsil {request.TehsilName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Tehsil: {request.TehsilName}", ex);
                throw;
            }
        }

        public async Task DeleteAsync(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"TehsilId", id },
                    {"IsStatus", false },
                    {"ModifyBy", userId },
                    {"ActionType" , (int)CrudActionType.Delete }
                };

                var response = await repository.DeleteAsync(TehsilQueries.AddTehsil, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Tehsil with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Tehsil id: {id}", ex);
                throw;
            }

        }


    }
}
