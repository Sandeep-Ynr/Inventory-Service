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
    public class DistrictService : IDistrictService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;

        public DistrictService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory)
        {
            this.logging = logging.ForContext("ServiceName", nameof(DistrictService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));

        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(DistrictRequest request)
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"DistrictId", request.DistrictId},
                {"StateId", request.StateId },
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<CommonLists>(DistrictQueries.GetDistrict, requestParams, null, CommandType.StoredProcedure);

            return response;

        }
        public async Task<string> AddDistrictsAsync(DistrictInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                //{ "DistrictId", request.DistrictId ?? (object)DBNull.Value },
                { "DistrictName", request.DistrictName ?? (object)DBNull.Value }, 
                { "StateId", request.StateId ?? (object)DBNull.Value },
                { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
            };

                var response = await repository.QueryAsync<CommonLists>(
                    DistrictQueries.AddDistrict, requestParams, null, CommandType.StoredProcedure
                );

                // Return the inserted StateId or Name, etc. depending on your SP response
                return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return "Error occurred";
            }
        }
        public async Task<string> UpdateDistrictAsync(DistrictUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)CrudActionType.Update },
                { "DistrictId", request.DistrictId},
                { "DistrictName", request.DistrictName ?? (object)DBNull.Value },
                { "StateId", request.StateId ?? (object)DBNull.Value },
                { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                { "ModifyBy", request.ModifyBy }
            
                //{ "StateId", request.StateId ?? (object)DBNull.Value },
                //{ "ModifyBy", request.ModifyBy ?? (object)DBNull.Value },
                //UpdateAsync
            };

               /* var response = await repository.QueryAsync<CommonLists>(
                    DistrictQueries.AddDistrict, requestParams, null, CommandType.StoredProcedure
                );*/
                var response = await repository.QueryAsync<CommonLists>(
                    DistrictQueries.AddDistrict, requestParams, null, CommandType.StoredProcedure
                );

                return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return "Error occurred";
            }
        }
        public async Task<DistrictResponse?> GetByIdAsync(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for user id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<DistrictResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<DistrictResponse>(DistrictQueries.GetDistrict, new Dictionary<string, object> { { "DistrictId", id } }, null);

                var result = data.Any() ? data.FirstOrDefault() : new DistrictResponse();
                logging.LogInfo(result != null
                    ? $"User with id {id} retrieved successfully."
                    : $"User with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for user id: {id}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<DistrictResponse>> GetDistricts(DistrictRequest request) 
        { 
            var repository = repositoryFactory.Connect<DistrictResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"DistrictId", request.DistrictId},
                {"StateId", request.StateId },
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<DistrictResponse>(DistrictQueries.GetDistrict, requestParams, null, CommandType.StoredProcedure);

            return response;
        }
        public async Task<string> DeleteAsync(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"DistrictId", id },
                    {"IsStatus", false },
                    {"ModifyBy", userId },
                    {"ActionType" , (int)CrudActionType.Delete }
                };

                var response = await repository.QueryAsync<CommonLists>(
                   DistrictQueries.AddDistrict, requestParams, null, CommandType.StoredProcedure
                );

                return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return "Error occurred";
            }

        }

    }
}
