using System.Data;
using System.Text.Json;
using Azure.Core;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Dtos;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.Bmc;
using MilkMatrix.Milk.Models.Request.Bmc;
using MilkMatrix.Milk.Models.Response.Bmc;
using static MilkMatrix.Milk.Models.Queries.BmcQueries;

namespace MilkMatrix.Milk.Implementations.Bmc
{
    public class BmcService : IBmcService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;
        
        public BmcService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(BmcService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddAsync(BmcInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {   
                    { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                    { "BmcName", request.BmcName ?? (object)DBNull.Value },
                    { "BmcCode", request.BmcCode ?? (object)DBNull.Value },
                    { "BusinessEntityId",request.BusinessEntityId ?? (object)DBNull.Value },
                    { "MccId",request.MccId  },
                    { "RegionalName", request.RegionalName ?? (object)DBNull.Value },
                    { "Capacity", request.Capacity ?? (object)DBNull.Value },
                    { "Manufacturer", request.Manufacturer ?? (object)DBNull.Value },
                    { "Model", request.Model ?? (object)DBNull.Value },
                    { "InstallationDate", request. InstallationDate ?? (object)DBNull.Value },
                    { "SerialNo", request.SerialNo ?? (object)DBNull.Value },
                    { "Remarks", request.Remarks ?? (object)DBNull.Value },
                    { "AnimalTypeId", request.AnimalTypeId ?? (object)DBNull.Value },
                    { "FsssiNumber", request.FSSSINumber ?? (object)DBNull.Value },
                    { "MappedMppId", request.MappedMppId ?? (object)DBNull.Value },
                    { "HasExtraTank", request.HasExtraTank ?? (object)DBNull.Value },
                    { "Address", request.Address ?? (object)DBNull.Value },
                    { "StateId", request.StateId },
                    { "DistrictId", request.DistrictId },
                    { "TehsilId", request.TehsilId },
                    { "VillageId", request.VillageId },
                    { "HamletId", request.HamletId },
                    { "Pincode", request.Pincode },
                    { "Latitude", request.Latitude ?? (object)DBNull.Value },
                    { "Longitude", request.Longitude ?? (object)DBNull.Value },
                    { "ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
                    { "MobileNo", request.MobileNo ?? (object)DBNull.Value },
                    { "EmailID", request.EmailId ?? (object)DBNull.Value },
                    { "IsWorking", request.IsWorking ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                };
                var message = await repository.AddAsync(BmcQuery.AddBmc, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"BMC {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for BMC: {request.BmcName}", ex);
                throw;
            }
        }

        public async Task UpdateAsync(BmcUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "BmcId", request.BmcId},
                    { "BmcName", request.BmcName ?? (object)DBNull.Value },
                    { "BmcCode", request.BmcCode ?? (object)DBNull.Value },
                    { "BusinessEntityId",request.BusinessEntityId ?? (object)DBNull.Value },
                    { "MccId",request.MccId  },
                    { "RegionalName", request.RegionalName ?? (object)DBNull.Value },
                    { "Capacity", request.Capacity ?? (object)DBNull.Value },
                    { "Manufacturer", request.Manufacturer ?? (object)DBNull.Value },
                    { "Model", request.Model ?? (object)DBNull.Value },
                    { "InstallationDate", request. InstallationDate ?? (object)DBNull.Value },
                    { "SerialNo", request.SerialNo ?? (object)DBNull.Value },
                    { "Remarks", request.Remarks ?? (object)DBNull.Value },
                    { "AnimalTypeId", request.AnimalTypeId ?? (object)DBNull.Value },
                    { "FsssiNumber", request.FSSSINumber ?? (object)DBNull.Value },
                    { "MappedMppId", request.MappedMppId ?? (object)DBNull.Value },
                    { "HasExtraTank", request.HasExtraTank ?? (object)DBNull.Value },
                    { "Address", request.Address ?? (object)DBNull.Value },
                    { "StateId", request.StateId },
                    { "DistrictId", request.DistrictId },
                    { "TehsilId", request.TehsilId },
                    { "VillageId", request.VillageId },
                    { "HamletId", request.HamletId },
                    { "Pincode", request.Pincode },
                    { "Latitude", request.Latitude ?? (object)DBNull.Value },
                    { "Longitude", request.Longitude ?? (object)DBNull.Value },
                    { "ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
                    { "MobileNo", request.MobileNo ?? (object)DBNull.Value },
                    { "EmailID", request.EmailId ?? (object)DBNull.Value },
                    { "IsWorking", request.IsWorking ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                await repository.UpdateAsync(BmcQuery.AddBmc, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"BMC {request.BmcName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for BMC: {request.BmcName}", ex);
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
                    {"BmcId", id },
                    {"IsActive", false },
                    {"ModifyBy", userId }

                };

                var response = await repository.DeleteAsync(BmcQuery.AddBmc, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"BMC with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for BMC id: {id}", ex);
                throw;
            }

        }

        public async Task<IListsResponse<BmcResponse>> GetAllAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All },
                { "Start", request.Limit },
                { "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<BmcResponse, int, FiltersMeta>(BmcQuery.GetBmcList,
                    DbConstants.Main, parameters, null);

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
            return new ListsResponse<BmcResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<BmcIndividualResponse?> GetByIdAsync(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for BMC id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<BmcIndividualResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<BmcIndividualResponse>(BmcQuery.GetBmcList, new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "BmcId", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new BmcIndividualResponse();
                logging.LogInfo(result != null
                    ? $"BMC with id {id} retrieved successfully."
                    : $"BMC with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for BMC id: {id}", ex);
                throw;
            }
        }

    }
}
