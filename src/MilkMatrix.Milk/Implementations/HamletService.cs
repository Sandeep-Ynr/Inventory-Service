using System.Data;
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
        private readonly IQueryMultipleData queryMultipleData;
        public HamletService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(HamletService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
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
            try
            {
                logging.LogInfo($"GetByIdAsync called for Hamlet id: {hamletId}");
                var repo = repositoryFactory
                           .ConnectDapper<HamletResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<HamletResponse>(HamletQueries.GetHamlet, new Dictionary<string, object>
                {
                    { "ActionType",2},
                    { "HamletId", hamletId }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new HamletResponse();
                logging.LogInfo(result != null
                    ? $"Hamlet with id {hamletId} retrieved successfully."
                    : $"Hamlet with id {hamletId} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Hamlet id: {hamletId}", ex);
                throw;
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

        public async Task<IListsResponse<HamletResponse>> GetAllAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<HamletResponse, int, FiltersMeta>(HamletQueries.GetHamletList,
                    DbConstants.Main,
                    parameters,
                    null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<HamletResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }

}
