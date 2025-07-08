using System.Data;
using Azure.Core;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Contracts.Bank;
using MilkMatrix.Milk.Models.Request.Bank;
using MilkMatrix.Milk.Models.Response.Bank;
using static MilkMatrix.Milk.Models.Queries.BankQueries;
namespace MilkMatrix.Milk.Implementations
{
    public class BankTypeService : IBankTypeService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;

        public BankTypeService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory)
        {
            this.logging = logging.ForContext("ServiceName", nameof(DistrictService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        }
        public async Task AddBankType(BankTypeInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                { "BankTypeName", request.BankTypeName ?? (object)DBNull.Value },
                { "BankTypeDescription", request.BankTypeDescription ?? (object)DBNull.Value },
                { "IsStatus", request.IsActive },
                { "CreatedBy", request.CreatedBy  },
            };

                var response = await repository.AddAsync(BankTypeQueries.AddBankType, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Bank Type {request.BankTypeName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Bank Type: {request.BankTypeName}", ex);
                throw;
            }
        }
        public async Task UpdateBankType(BankTypeUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                    {
                        { "ActionType", (int)CrudActionType.Update},
                        { "BankTypeId", request.BankTypeId },
                        { "BankTypeName", request.BankTypeName ?? (object)DBNull.Value },
                        { "BankTypeDescription", request.BankTypeDescription ?? (object)DBNull.Value },
                        { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                        { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value }
                    };

                await repository.UpdateAsync(BankTypeQueries.AddBankType, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Bank Type {request.BankTypeName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Bank Type Name: {request.BankTypeName}", ex);
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
                    {"ActionType" , (int)CrudActionType.Delete },
                    {"BankTypeId", id },
                    {"ModifyBy", userId }
                };

                var response = await repository.DeleteAsync(
                   BankTypeQueries.AddBankType, requestParams, CommandType.StoredProcedure
                );

                logging.LogInfo($"Bank Type with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Bank Type id: {id}", ex);
                throw;
            }

        }
        public async Task<BankTypeResponse?> GetByBankTypeId(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Tehsil id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<BankTypeResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<BankTypeResponse>(BankTypeQueries.GetBankType, new Dictionary<string, object>
                {
                    { "ActionType", 2 },
                    { "BankTypeID", id },
                    { "IsStatus", true }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new BankTypeResponse();
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
        public async Task<IEnumerable<BankTypeResponse>> GetBankTypes(BankTypeRequest request)
        {
            var repository = repositoryFactory.Connect<BankTypeResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
                {
                    {"BankTypeId",(int)request.BankTypeId },
                    {"ActionType",(int)request.ActionType },
                    {"IsStatus", request.IsActive}
                };
            var response = await repository.QueryAsync<BankTypeResponse>(BankTypeQueries.GetBankType, requestParams, null, CommandType.StoredProcedure);
            return response;
        }
        public async Task<IEnumerable<CommonLists>> GetSpecificLists(BankTypeRequest request)
        {
            var repository = repositoryFactory.Connect<BankTypeResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
                {
                    {"ActionType",(int)request.ActionType },
                    {"IsStatus", request.IsActive}
                };
            var response = await repository.QueryAsync<CommonLists>(BankTypeQueries.GetBankType, requestParams, null, CommandType.StoredProcedure);
            return response;
        }

      
    }
}
