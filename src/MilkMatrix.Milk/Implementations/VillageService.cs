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
         private readonly IQueryMultipleData queryMultipleData;

        public VillageService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(VillageService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(VillageService));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
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
                var data = await repo.QueryAsync<VillageResponse>(VillageQueries.GetVillage, new Dictionary<string, object> {
                    { "ActionType",2 },
                    { "VillageId", villageId } 
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new VillageResponse();

                if (result != null && result.Id > 0)
                {
                    logging.LogInfo($"User with id {villageId} retrieved successfully.");
                    return result;
                }
                else
                {
                    logging.LogInfo($"User with id {villageId} not found.");
                    return null;
                }
                

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

                logging.LogInfo($"Village with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Village id: {id}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<VillageResponse>> GetAllAsync(IListsRequest request)
        {

            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<VillageResponse, int, FiltersMeta>(VillageQueries.GetVillageList,
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
            return new ListsResponse<VillageResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
