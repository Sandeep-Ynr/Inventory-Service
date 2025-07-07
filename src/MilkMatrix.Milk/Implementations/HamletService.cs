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
    public class HamletService : IHamletService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;

        public HamletService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory)
        {
            this.logging = logging.ForContext("ServiceName", nameof(HamletService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(HamletRequest request)
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",1 },
                {"HamletId", request.HamletId},
                {"VillageId", request.VillageId},
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<CommonLists>(HamletQueries.GetHamlet, requestParams, null, CommandType.StoredProcedure);

            return response;
        }


        public async Task<IEnumerable<HamletResponse>> GetHamlets(HamletRequest request)
        {
            var repository = repositoryFactory.Connect<HamletResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"VillageId", request.VillageId},
                {"HamletId", request.HamletId },
                {"IsStatus", request.IsActive}
            };
            var response = await repository.QueryAsync<HamletResponse>(HamletQueries.GetHamlet, requestParams, null, CommandType.StoredProcedure);
            return response;
        }


        public async Task<HamletResponse?> GetByHamletId(int hamletId)
        {
            logging.LogInfo($"GetByIdAsync called for user id: {hamletId}");
            var repo = repositoryFactory
                       .ConnectDapper<HamletResponse>(DbConstants.Main);
            var data = await repo.QueryAsync<HamletResponse>(HamletQueries.GetHamlet, new Dictionary<string, object> {
                    { "ActionType",2 },
                    { "HamletId", hamletId }
                }, null);

            var result = data.Any() ? data.FirstOrDefault() : new HamletResponse();

            if (result != null && result.HamletId > 0)
            {
                logging.LogInfo($"User with id {hamletId} retrieved successfully.");
                return result;
            }
            else
            {
                logging.LogInfo($"User with id {hamletId} not found.");
                return null;
            }
        }


        public async Task AddHamlet(HamletInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
            {
                 { "ActionType",  (int)CrudActionType.Create },
                 { "HamletName", request.HamletName ?? (object)DBNull.Value },
                 { "VillageId", request.VillageId ?? (object)DBNull.Value },
                 { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                 { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },

            };
                var response = await repository.AddAsync(HamletQueries.AddHamlet, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Hamlet {request.HamletName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Hamlet: {request.HamletName}", ex);
                throw;
            }
        }

        public async Task UpdateHamlet(HamletUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
            {
                 { "ActionType", 2 },
                 { "HamletId", request.HamletId ?? (object)DBNull.Value },
                 { "HamletName", request.HamletName ?? (object)DBNull.Value },
                 { "VillageId", request.VillageId ?? (object)DBNull.Value },
                 { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                 { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value },
            };
                await repository.UpdateAsync(HamletQueries.AddHamlet, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"District {request.HamletName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Hamlet: {request.HamletName}", ex);
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
                    {"HamletID", id },
                    {"IsStatus", false },
                    {"ModifyBy", userId },
                    {"ActionType" , (int)CrudActionType.Delete }
                };
                var response = await repository.DeleteAsync(
                     HamletQueries.AddHamlet, requestParams, CommandType.StoredProcedure
                  );
                logging.LogInfo($"Hamlet with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Hamlet id: {id}", ex);
                throw;
            }
        }

    }
}
