using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
using MilkMatrix.Milk.Contracts.Milk.Transaction.FarmerCollection;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Milk.Transaction.FarmerCollection;
using MilkMatrix.Milk.Models.Response.Milk.Transaction.FarmerCollection;
using static MilkMatrix.Milk.Models.Queries.MilkQueries;


namespace MilkMatrix.Milk.Implementations.Milk.FarmerCollection
{
    public class FarmerCollectionService : IFarmerCollectionService
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;
        private readonly ILogging logging;

        public FarmerCollectionService(
            IRepositoryFactory repositoryFactory,
            IQueryMultipleData queryMultipleData,
            ILogging logging)
        {
            this.repositoryFactory = repositoryFactory;
            this.queryMultipleData = queryMultipleData;
            this.logging = logging;
        }

        public async Task InsertFarmerColl(FarmerCollectionInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "BusinessId", request.BusinessId },
                    { "DumpDate", request.DumpDate },
                    { "DumpTime", request.DumpTime ?? (object)DBNull.Value },
                    { "FarmerId", request.FarmerId },
                    { "Fat", request.Fat ?? (object)DBNull.Value },
                    { "Snf", request.Snf ?? (object)DBNull.Value },
                    { "LR", request.LR ?? (object)DBNull.Value },
                    { "WeightLiter", request.WeightLiter },
                    { "Type", request.Type ?? (object)DBNull.Value },
                    { "Rtpl", request.Rtpl ?? (object)DBNull.Value },
                    { "TotalAmount", request.TotalAmount ?? (object)DBNull.Value },
                    { "SampleId", request.SampleId ?? (object)DBNull.Value },
                    { "BatchNo", request.BatchNo ?? (object)DBNull.Value },
                    { "FarmerName", request.FarmerName ?? (object)DBNull.Value },
                    { "Mobile", request.Mobile ?? (object)DBNull.Value },
                    { "CompanyCode", request.CompanyCode ?? (object)DBNull.Value },
                    { "IMEI_No", request.IMEI_No ?? (object)DBNull.Value },
                    { "bmc_id", request.BmcId },
                    { "mcc_id", request.MccId },
                    { "Shift", request.Shift },
                    { "Status", request.Status ?? "APPROVED" },
                    { "StatusID", request.StatusId },
                    { "CId", request.CId },
                    { "CDate", request.CDate ?? DateTime.Now },
                    { "ApprovedBy", request.ApprovedBy ?? (object)DBNull.Value },
                    { "ApprovedDate", request.ApprovedDate ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus ?? true },
                    { "CreatedBy", request.CreatedBy ?? 0 }
                };

                var message = await repository.AddAsync(
                    FarmerCollectionQueries.AddFarmerCollection,
                    requestParams,
                    CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");
                else
                    logging.LogInfo($"FarmerCollection {message} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddFarmerCollection for FarmerId: {request.FarmerId}", ex);
                throw;
            }
        }

        public async Task UpdateFarmerColl(FarmerCollectionUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "FarmerCollectionId", request.FarmerCollectionId },
                    { "BusinessId", request.BusinessId },
                    { "DumpDate", request.DumpDate },
                    { "DumpTime", request.DumpTime ?? (object)DBNull.Value },
                    { "FarmerId", request.FarmerId },
                    { "Fat", request.Fat ?? (object)DBNull.Value },
                    { "Snf", request.Snf ?? (object)DBNull.Value },
                    { "LR", request.LR ?? (object)DBNull.Value },
                    { "WeightLiter", request.WeightLiter },
                    { "Type", request.Type ?? (object)DBNull.Value },
                    { "Rtpl", request.Rtpl ?? (object)DBNull.Value },
                    { "TotalAmount", request.TotalAmount ?? (object)DBNull.Value },
                    { "SampleId", request.SampleId ?? (object)DBNull.Value },
                    { "BatchNo", request.BatchNo ?? (object)DBNull.Value },
                    { "FarmerName", request.FarmerName ?? (object)DBNull.Value },
                    { "Mobile", request.Mobile ?? (object)DBNull.Value },
                    { "CompanyCode", request.CompanyCode ?? (object)DBNull.Value },
                    { "IMEI_No", request.IMEI_No ?? (object)DBNull.Value },
                    { "bmc_id", request.BmcId },
                    { "mcc_id", request.MccId },
                    { "Shift", request.Shift },
                    { "Status", request.Status },
                    { "StatusID", request.StatusId },
                    { "CId", request.CId },
                    { "ModifiedBy", request.ModifyBy ?? 0 },
                };

                var message = await repository.UpdateAsync(
                    FarmerCollectionQueries.AddFarmerCollection,
                    requestParams,
                    CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");
                else
                    logging.LogInfo($"FarmerCollection {request.FarmerCollectionId} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateFarmerCollection for Id: {request.FarmerCollectionId}", ex);
                throw;
            }
        }

        public async Task DeleteFarmerColl(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "FarmerCollectionId", id }
                };

                var response = await repository.DeleteAsync(
                    FarmerCollectionQueries.AddFarmerCollection,
                    requestParams,
                    CommandType.StoredProcedure);

                logging.LogInfo($"FarmerCollection with id {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for FarmerCollection id: {id}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<FarmerCollectionResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
                };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<FarmerCollectionResponse, int, FiltersMeta>(
                    FarmerCollectionQueries.GetFarmerCollectionList,
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
            return new ListsResponse<FarmerCollectionResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }


        public async Task<FarmerCollectionResponse?> GetById(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for FarmerCollection id: {id}");
                var repo = repositoryFactory.ConnectDapper<FarmerCollectionResponse>(DbConstants.Main);

                var data = await repo.QueryAsync<FarmerCollectionResponse>(
                    FarmerCollectionQueries.GetFarmerCollectionList,
                    new Dictionary<string, object>
                    {
                        { "ActionType", (int)ReadActionType.Individual },
                        { "FarmerCollectionId", id }
                    },
                    null);

                var result = data.Any() ? data.FirstOrDefault() : new FarmerCollectionResponse();
                logging.LogInfo(result != null
                    ? $"FarmerCollection with id {id} retrieved successfully."
                    : $"FarmerCollection with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for FarmerCollection id: {id}", ex);
                throw;
            }
        }

        public Task<IEnumerable<CommonLists>> GetSpecificLists(FarmerCollectionRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FarmerCollectionResponse>> GetFarmerCollections(FarmerCollectionRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
