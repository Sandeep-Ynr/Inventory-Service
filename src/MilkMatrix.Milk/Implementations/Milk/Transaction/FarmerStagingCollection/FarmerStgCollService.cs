using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Azure;
using Microsoft.Extensions.Options;
using MilkMatrix.Api.Models.Request.Milk.Transaction.FarmerStagingCollection;
using MilkMatrix.Api.Models.Request.MilkCollection;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Entities.Response.Rejection;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.Milk.DockData;
using MilkMatrix.Milk.Contracts.Milk.MilkCollection;
using MilkMatrix.Milk.Contracts.Milk.Transaction.FarmerStagingCollection;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Milk.DockData;
using MilkMatrix.Milk.Models.Request.Milk.Transaction.FarmerStagingCollection;
using MilkMatrix.Milk.Models.Response.Milk.DockData;
using MilkMatrix.Milk.Models.Response.MPP;
using static MilkMatrix.Milk.Models.Queries.MilkQueries;
using static MilkMatrix.Milk.Models.Queries.PriceQueries;

namespace MilkMatrix.Milk.Implementations.Milk.Transaction.FarmerStagingCollection
{
    public class FarmerStgCollService : IFarmerStagingCollectionService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public FarmerStgCollService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(FarmerStgCollService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }


        public async Task DeleteFarmerCollection(long id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "CollecionID", id },   // 🔹 Changed from DockDataUpdateId
                    { "ModifiedBy", userId }
                };

                var result = await repository.DeleteAsync(FarmerStgQueries.AddFarmerStg, requestParams, CommandType.StoredProcedure);

                if (result.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {result}");
                }

                logging.LogInfo($"FarmerStg with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteFarmerStg for ID: {id}", ex);
                throw;
            }
        }


        public async Task ImportFarmerCollection(FarmerCollStgInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "RowId", request.RowId },
                    { "DumpDate", request.DumpDate },
                    { "DumpTime", request.DumpTime ?? (object)DBNull.Value },
                    { "BusinessEntityId", request.BusinessEntityId },
                    { "Mppcode", request.Mppcode ?? (object)DBNull.Value },
                    { "BatchNo", request.BatchNo ?? (object)DBNull.Value },
                    { "ReferenceId", request.ReferenceId ?? (object)DBNull.Value },
                    { "FarmerId", request.FarmerId ?? (object)DBNull.Value },
                    { "FName", request.FName ?? (object)DBNull.Value },
                    { "Shift", request.Shift ?? (object)DBNull.Value },
                    { "Type", request.Type ?? (object)DBNull.Value },
                    { "WeightLiter", request.WeightLiter },
                    { "Fat", request.Fat ?? (object)DBNull.Value },
                    { "Snf", request.Snf ?? (object)DBNull.Value },
                    { "LR", request.LR ?? (object)DBNull.Value },
                    { "Can", request.Can ?? (object)DBNull.Value },
                    { "Rtpl", request.Rtpl ?? (object)DBNull.Value },
                    { "TotalAmount", request.TotalAmount ?? (object)DBNull.Value },
                    { "SampleId", request.SampleId ?? (object)DBNull.Value },
                    { "IsQltyAuto", request.IsQltyAuto ?? (object)DBNull.Value },
                    { "IsQtyAuto", request.IsQtyAuto ?? (object)DBNull.Value },
                    { "InsertMode", request.InsertMode ?? "IMP" },
                    { "IMEI_No", request.IMEI_No ?? (object)DBNull.Value },
                    { "IsValidated", request.IsValidated },
                    { "IsProcess", request.IsProcess },
                    { "ProcessDate", request.ProcessDate ?? (object)DBNull.Value },
                    { "is_status", request.IsStatus ?? (object)DBNull.Value },
                    { "is_deleted", request.IsDeleted ?? (object)DBNull.Value },
                    { "created_by", request.CreatedBy ?? 0 },
                    { "created_on", request.CreatedOn ?? DateTime.Now },
                    { "deleted_by", request.DeletedBy ?? (object)DBNull.Value },
                    { "deleted_on", request.DeletedOn ?? (object)DBNull.Value }
                };

                var message = await repository.AddAsync(FarmerStgQueries.AddFarmerStg, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Farmer Collection {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddFarmerCollection", ex);
                throw;
            }
        }
        public async Task UpdateFarmerCollection(FarmerCollStgUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "RowId", request.RowId },
                    { "DumpDate", request.DumpDate },
                    { "DumpTime", request.DumpTime ?? (object)DBNull.Value },
                    { "BusinessEntityId", request.BusinessEntityId },
                    { "Mppcode", request.Mppcode ?? (object)DBNull.Value },
                    { "BatchNo", request.BatchNo ?? (object)DBNull.Value },
                    { "ReferenceId", request.ReferenceId ?? (object)DBNull.Value },
                    { "FarmerId", request.FarmerId ?? (object)DBNull.Value },
                    { "FName", request.FName ?? (object)DBNull.Value },
                    { "Shift", request.Shift ?? (object)DBNull.Value },
                    { "Type", request.Type ?? (object)DBNull.Value },
                    { "WeightLiter", request.WeightLiter },
                    { "Fat", request.Fat ?? (object)DBNull.Value },
                    { "Snf", request.Snf ?? (object)DBNull.Value },
                    { "LR", request.LR ?? (object)DBNull.Value },
                    { "Can", request.Can ?? (object)DBNull.Value },
                    { "Rtpl", request.Rtpl ?? (object)DBNull.Value },
                    { "TotalAmount", request.TotalAmount ?? (object)DBNull.Value },
                    { "SampleId", request.SampleId ?? (object)DBNull.Value },
                    { "IsQltyAuto", request.IsQltyAuto ?? (object)DBNull.Value },
                    { "IsQtyAuto", request.IsQtyAuto ?? (object)DBNull.Value },
                    { "InsertMode", request.InsertMode ?? "IMP" },
                    { "IMEI_No", request.IMEI_No ?? (object)DBNull.Value },
                    { "IsValidated", request.IsValidated },
                    { "IsProcess", request.IsProcess },
                    { "ProcessDate", request.ProcessDate ?? (object)DBNull.Value },
                    { "is_status", request.IsStatus ?? (object)DBNull.Value },
                    { "is_deleted", request.IsDeleted ?? (object)DBNull.Value },
                    { "created_by", request.CreatedBy ?? 0 },
                    { "created_on", request.CreatedOn ?? DateTime.Now },
                    { "deleted_by", request.DeletedBy ?? (object)DBNull.Value },
                    { "deleted_on", request.DeletedOn ?? (object)DBNull.Value }
                };

                var message = await repository.UpdateAsync(FarmerStgQueries.AddFarmerStg, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }

                logging.LogInfo($"Farmer Collection with RowId {request.RowId} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error updating FarmerCollStg record with RowId: {request.RowId}", ex);
                throw;
            }
        }


        public async Task<FarmerCollResponse?> GetFarmerCollectionExportById(long id)
        {
            try
            {
                logging.LogInfo($"GetById called for FarmerCollection ID: {id}");
                var repo = repositoryFactory.ConnectDapper<FarmerCollResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<FarmerCollResponse>(
                    FarmerStgQueries.GetFarmerStgList,
                    new Dictionary<string, object>
                    {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "RowId", id }
                    },
                    null);

                var result = data.FirstOrDefault();
                logging.LogInfo(result != null
                    ? $"FarmerCollection with ID {id} retrieved successfully."
                    : $"FarmerCollection with ID {id} not found.");

                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetById for FarmerCollection ID: {id}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<FarmerCollResponse>> GetFarmerCollectionExport(IListsRequest request)
        {
   
                logging.LogInfo("GetFarmerCollectionExport called.");

                var repository = repositoryFactory.Connect<FarmerCollResponse>(DbConstants.Main);

                var parameters = new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.All }
                    //{ "Start", request.Start },
                    //{ "End", request.End }
                };

                // 1. Fetch all results, count, and filter meta from stored procedure
                var (allResults, countResult, filterMetas) = await queryMultipleData
                    .GetMultiDetailsAsync<FarmerCollResponse, int, FiltersMeta>(FarmerStgQueries.GetFarmerStgList,
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
                return new ListsResponse<FarmerCollResponse>
                {
                    Count = filteredCount,
                    Results = paged.ToList(),
                    Filters = filterMetas
                };
        }
        //public async Task<IListsResponse<FarmerCollResponse>> GetFarmerCollectionExport(IListsRequest request)
        //{
        //    var parameters = new Dictionary<string, object>
        //    {
        //        { "ActionType", (int)ReadActionType.All }
        //    };

            //    var (allResults, countResult, filterMetas) = await queryMultipleData
            //        .GetMultiDetailsAsync<FarmerCollResponse, int, FiltersMeta>(
            //            FarmerStgQueries.GetFarmerStgList,
            //            DbConstants.Main,
            //            parameters,
            //            null);

            //    var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            //    var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            //    var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            //    var filtered = allResults.AsQueryable().ApplyFilters(filters);
            //    var sorted = filtered.ApplySorting(sorts);
            //    var paged = sorted.ApplyPaging(paging);

            //    var filteredCount = filtered.Count();

            //    //var list = JsonSerializer.Deserialize<List<FarmerCollectionStagingDetail>>();

            //    return new ListsResponse<FarmerCollResponse>
            //    {
            //        Count = filteredCount,
            //        Results = paged.ToList(),
            //        Filters = filterMetas
            //    };
            //}


   }
 }

