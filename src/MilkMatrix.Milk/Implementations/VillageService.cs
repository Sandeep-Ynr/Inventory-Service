using System.Data;
using Azure.Core;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Models.Request;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;
using static MilkMatrix.Milk.Models.Queries.GeographicalQueries;

namespace MilkMatrix.Milk.Implementations
{
    public class VillageService : IVillageService
    {
        private readonly ILogging logging;

        private readonly AppConfig appConfig;

        private readonly IRepositoryFactory repositoryFactory;

        public VillageService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory)
        {
            this.logging = logging.ForContext("ServiceName", nameof(VillageService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(VillageService));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(VillageRequest request)
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"VillageId", request.VillageId},
                {"TehsilId", request.TehsilId },
                {"IsStatus", request.IsActive}
            };
            var response = await repository.QueryAsync<CommonLists>(VillageQueries.GetVillage, requestParams, null, CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<VillageResponse>> GetVillages(VillageRequest request)
        {
            var repository = repositoryFactory.Connect<VillageResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"VillageId", request.VillageId},
                {"TehsilId", request.TehsilId },
                {"IsStatus", request.IsActive}
            };
            var response = await repository.QueryAsync<VillageResponse>(VillageQueries.GetVillage, requestParams, null, CommandType.StoredProcedure);
            return response;
        }

        public async Task<VillageResponse?> GetByVillageId(int villageId)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for user id: {villageId}");
                var repo = repositoryFactory
                           .ConnectDapper<VillageResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<VillageResponse>(VillageQueries.GetVillage, new Dictionary<string, object> { { "VillageId", villageId } }, null);

                var result = data.Any() ? data.FirstOrDefault() : new VillageResponse();
                logging.LogInfo(result != null
                    ? $"User with id {villageId} retrieved successfully."
                    : $"User with id {villageId} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for user id: {villageId}", ex);
                throw;
            }
        }
        public async Task AddVillage(VillageInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "VillageName", request.VillageName ?? (object)DBNull.Value },
                    { "TehsilId", request.TehsilId ?? (object)DBNull.Value},
                    { "IsStatus", request.IsActive?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value},
                };
                var response = await repository.AddAsync(VillageQueries.AddVillage, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Village {request.VillageName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Village: {request.VillageName}", ex);
                throw;
            }
        }


        public async Task UpdateVillage(VillageUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", 2 }, // 2 = Update
                    { "VillageId", request.VillageId ?? (object)DBNull.Value},
                    { "VillageName", request.VillageName ?? (object)DBNull.Value },
                    { "TehsilId", request.TehsilId?? (object)DBNull.Value },
                    { "IsStatus", request.IsActive?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value}
                };
                await repository.UpdateAsync(VillageQueries.AddVillage, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Village {request.VillageName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Village: {request.VillageName}", ex);
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
                    {"VillageID", id },
                    {"IsStatus", false },
                    {"ModifyBy", userId },
                    {"ActionType" , (int)CrudActionType.Delete }
                };

                var response = await repository.DeleteAsync(
                   VillageQueries.AddVillage , requestParams, CommandType.StoredProcedure
                );

                logging.LogInfo($"District with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for District id: {id}", ex);
                throw;
            }
        }
    }
}
