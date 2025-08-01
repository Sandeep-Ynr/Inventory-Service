using System.Data;
using Azure.Core;
using Microsoft.Extensions.Options;
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
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.Bank;
using MilkMatrix.Milk.Models.Request.Bank;
using MilkMatrix.Milk.Models.Response.Bank;
using MilkMatrix.Milk.Models.Response.Geographical;
using static MilkMatrix.Milk.Models.Queries.BankQueries;
using static MilkMatrix.Milk.Models.Queries.GeographicalQueries;
namespace MilkMatrix.Milk.Implementations
{
    public class BankTypeService : IBankTypeService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public BankTypeService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(DistrictService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }
        public async Task AddBankType(BankTypeInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                { "BusinessId ", request.BusinessId  ?? (object)DBNull.Value },
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
        public async Task Delete(int id, int userId)
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
        public async Task<BankTypeResponse?> GetById(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Bank Type id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<BankTypeResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<BankTypeResponse>(BankTypeQueries.GetBankTypeList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "BankTypeId", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new BankTypeResponse();
                logging.LogInfo(result != null
                    ? $"Bank Type with id {id} retrieved successfully."
                    : $"Bank Type with id {id} not found.");
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
            var response = await repository.QueryAsync<BankTypeResponse>(BankTypeQueries.GetBankTypeList, requestParams, null, CommandType.StoredProcedure);
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
            var response = await repository.QueryAsync<CommonLists>(BankTypeQueries.GetBankTypeList, requestParams, null, CommandType.StoredProcedure);
            return response;
        }

        public async Task<IListsResponse<BankTypeResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<BankTypeResponse, int, FiltersMeta>(BankTypeQueries.GetBankTypeList,
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
            return new ListsResponse<BankTypeResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
