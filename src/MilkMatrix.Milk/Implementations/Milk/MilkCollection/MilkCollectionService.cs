using System.Data;
using Azure.Core;
using Microsoft.Extensions.Options;
using MilkMatrix.Api.Models.Request.MilkCollection;
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
using MilkMatrix.Milk.Contracts.Milk.MilkCollection;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Bank;
using MilkMatrix.Milk.Models.Response.Bank;
using MilkMatrix.Milk.Models.Response.Geographical;
using MilkMatrix.Milk.Models.Response.MPP;
using static MilkMatrix.Milk.Models.Queries.BankQueries;
using static MilkMatrix.Milk.Models.Queries.GeographicalQueries;
using static MilkMatrix.Milk.Models.Queries.MilkQueries;
namespace MilkMatrix.Milk.Implementations
{
    public class MilkCollectionService : IMilkCollectionService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public MilkCollectionService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(DistrictService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task<MilkCollectionResponse?> GetMilkCollectionById(int id)
        {
            try
            {
                logging.LogInfo($"GetMilkCollectionById called for ID: {id}");
                var repo = repositoryFactory.ConnectDapper<MilkCollectionResponse>(DbConstants.Main);

                var data = await repo.QueryAsync<MilkCollectionResponse>(
                    MilkCollectionQueries.GetMilkCollectionList,
                    new Dictionary<string, object>
                    {
                        { "ActionType", (int)ReadActionType.Individual },
                        { "CollectionId", id }
                    }, null
                );

                var result = data.FirstOrDefault() ?? new MilkCollectionResponse();
                logging.LogInfo(result != null
                    ? $"Milk Collection with ID {id} retrieved successfully."
                    : $"Milk Collection with ID {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetMilkCollectionById for ID: {id}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<MilkCollectionResponse>> GetMilkCollectionAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<MilkCollectionResponse, int, FiltersMeta>(MilkCollectionQueries.GetMilkCollectionList,
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
            return new ListsResponse<MilkCollectionResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

       

        public async Task InsertMilkCollection(MilkCollectionInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "BusinessID", request.BusinessID ?? (object)DBNull.Value },
                    { "MemberId", request.MemberId ?? (object)DBNull.Value },
                    { "CenterType", request.CenterType ?? (object)DBNull.Value },
                    { "CenterId", request.CenterId ?? (object)DBNull.Value },
                    { "RouteId", request.RouteId ?? (object)DBNull.Value },
                    { "CollectionDate", request.CollectionDate ?? (object)DBNull.Value },
                    { "Shift", request.Shift ?? (object)DBNull.Value },
                    { "MilkType", request.MilkType ?? (object)DBNull.Value },
                    { "QuantityLtr", request.QuantityLtr ?? (object)DBNull.Value },
                    { "Fat", request.Fat ?? (object)DBNull.Value },
                    { "Snf", request.Snf ?? (object)DBNull.Value },
                    { "RatePerLtr", request.RatePerLtr ?? (object)DBNull.Value },
                    { "Amount", request.Amount ?? (object)DBNull.Value },
                    { "CollectionMode", request.CollectionMode ?? (object)DBNull.Value },
                    { "Status", request.Status ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus ?? true },
                };
                var message = await repository.AddAsync(MilkCollectionQueries.AddMilkCollection, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Milk Collection added successfully: {message}");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in InsertMilkCollection for MemberId: {request.MemberId}", ex);
                throw;
            }
        }

        public async Task UpdateMilkCollection(MilkCollectionUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "CollectionId", request.CollectionId },
                    { "BusinessID", request.BusinessID ?? (object)DBNull.Value },
                    { "MemberId", request.MemberId ?? (object)DBNull.Value },
                    { "CenterType", request.CenterType ?? (object)DBNull.Value },
                    { "CenterId", request.CenterId ?? (object)DBNull.Value },
                    { "RouteId", request.RouteId ?? (object)DBNull.Value },
                    { "CollectionDate", request.CollectionDate ?? (object)DBNull.Value },
                    { "Shift", request.Shift ?? (object)DBNull.Value },
                    { "MilkType", request.MilkType ?? (object)DBNull.Value },
                    { "QuantityLtr", request.QuantityLtr ?? (object)DBNull.Value },
                    { "Fat", request.Fat ?? (object)DBNull.Value },
                    { "Snf", request.Snf ?? (object)DBNull.Value },
                    { "RatePerLtr", request.RatePerLtr ?? (object)DBNull.Value },
                    { "Amount", request.Amount ?? (object)DBNull.Value },
                    { "CollectionMode", request.CollectionMode ?? (object)DBNull.Value },
                    { "Status", request.Status ?? (object)DBNull.Value },
                    { "is_status", request.IsStatus ?? true },
                    { "is_deleted", request.IsDeleted ?? false },
                    { "ModifiedBy", request.ModifiedBy ?? 0 },
                    { "ModifiedOn", DateTime.Now }
                };
                var message = await repository.UpdateAsync(MilkCollectionQueries.AddMilkCollection, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Milk Collection record {request.CollectionId} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateMilkCollection for CollectionId: {request.CollectionId}", ex);
                throw;
            }
        }

        public async Task DeleteMilkCollection(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "CollectionId", id },
                    { "ModifiedBy", userId },
                };

                var response = await repository.DeleteAsync(
                  MilkCollectionQueries.AddMilkCollection, requestParams, CommandType.StoredProcedure
                );

                logging.LogInfo($"Milk Collection with id {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteMilkCollection for CollectionId: {id}", ex);
                throw;
            }
        }
    }

}


