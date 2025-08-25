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

      
        //public async Task DeleteFarmerCollection(long id, int userId)
        //{
        //    try
        //    {
        //        var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
        //        var requestParams = new Dictionary<string, object>
        //        {
        //            { "ActionType", (int)CrudActionType.Delete },
        //            { "CollecionID", id },   // 🔹 Changed from DockDataUpdateId
        //            { "ModifiedBy", userId }
        //        };

        //        var result = await repository.DeleteAsync(FarmerStgQueries.AddFarmerStg, requestParams, CommandType.StoredProcedure);
                
        //        if (result.StartsWith("Error"))
        //        {
        //            throw new Exception($"Stored Procedure Error: {result}");
        //        }

        //        logging.LogInfo($"FarmerStg with ID {id} deleted successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        logging.LogError($"Error in DeleteFarmerStg for ID: {id}", ex);
        //        throw;
        //    }
        //}


        public async Task ImportFarmerCollection(FarmerCollStgInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "HeaderId", request.HeaderId },
                    { "DumpDate", request.DumpDate },
                    { "DumpTime", request.DumpTime ?? (object)DBNull.Value },
                    { "Shift", request.Shift ?? (object)DBNull.Value },
                    { "BatchNo", request.BatchNo ?? (object)DBNull.Value },
                    { "MppId", request.MppId },
                    { "BmcId", request.BmcId },
                    { "InsertMode", request.InsertMode ?? "IMP" },
                    { "Status", request.Status ?? "PENDING" },
                    { "CompanyCode", request.CompanyCode ?? (object)DBNull.Value },
                    { "IMEI_No", request.ImeiNo ?? (object)DBNull.Value },
                    { "IsValidated", request.IsValidated },
                    { "IsProcess", request.IsProcess },
                    { "ProcessDate", request.ProcessDate ?? (object)DBNull.Value },
                    { "BusinessId", request.BusinessId },
                    { "CreatedOn", request.CreatedOn ?? DateTime.Now },
                    { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value },
                    { "ModifyOn", request.ModifyOn ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                    { "NewHeaderId", ""}
                };

                var message = await repository.AddAsync(FarmerStgQueries.AddFarmerStg, parameters, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"FarmerStg {message} added successfully." + message);
                }

                var tmpHeaderId = Convert.ToInt32(message);




                // 2. Loop through details and insert one-by-one
                foreach (var detail in request.Details)
                {
                    var detailParams = new Dictionary<string, object>
                    {
                        { "ActionType", (int)CrudActionType.Create },
                        { "HeaderId", tmpHeaderId },
                        { "FarmerId", detail.FarmerId ?? (object)DBNull.Value },
                        { "FarmerName", detail.FarmerName ?? (object)DBNull.Value },
                        { "Mobile", detail.Mobile ?? (object)DBNull.Value },
                        { "Fat", detail.Fat ?? (object)DBNull.Value },
                        { "Snf", detail.Snf ?? (object)DBNull.Value },
                        { "LR", detail.Lr ?? (object)DBNull.Value },
                        { "WeightLiter", detail.WeightLiter ?? (object)DBNull.Value },
                        { "Type", detail.Type ?? (object)DBNull.Value },
                        { "Rtpl", detail.RatePerLiter ?? (object)DBNull.Value },
                        { "TotalAmount", detail.TotalAmount ?? (object)DBNull.Value },
                        { "SampleId", detail.SampleId ?? (object)DBNull.Value },
                        { "Can", detail.Can ?? (object)DBNull.Value },
                        { "ReferenceId", detail.ReferenceId ?? (object)DBNull.Value },
                    };
                    var detailproc = await repository.AddAsync(FarmerStgQueries.AddFarmerStgDetail, detailParams, CommandType.StoredProcedure);
                    if (detailproc.StartsWith("Error"))
                    {
                        throw new Exception($"Stored Procedure Error: {detailproc}");
                    }
                    else
                    {
                        logging.LogInfo($"FarmerStg {detailproc} added successfully." + detailproc);
                    }


                    
                    //await repository.AddAsync("InsertMilkPriceDetail", detailParams, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error inserting FarmerCollStg record", ex);
                throw;
            }
        }
        //public async Task UpdateFarmerCollection(FarmerCollStgUpdateRequest request)
        //{
        //    try
        //    {
        //        var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

        //        var parameters = new Dictionary<string, object>
        //        {
        //            { "ActionType", (int)CrudActionType.Update },
        //            { "CollecionID", request.CollecionID },   // Primary key for update
        //            { "DumpDate", request.DumpDate },
        //            { "DumpTime", request.DumpTime ?? (object)DBNull.Value },
        //            { "FarmerId", request.FarmerId ?? (object)DBNull.Value },
        //            { "Fat", request.Fat ?? (object)DBNull.Value },
        //            { "Snf", request.Snf ?? (object)DBNull.Value },
        //            { "LR", request.LR ?? (object)DBNull.Value },
        //            { "WeightLiter", request.WeightLiter ?? (object)DBNull.Value },
        //            { "Type", request.Type ?? (object)DBNull.Value },
        //            { "Rtpl", request.Rtpl ?? (object)DBNull.Value },
        //            { "TotalAmount", request.TotalAmount ?? (object)DBNull.Value },
        //            { "SampleId", request.SampleId ?? (object)DBNull.Value },
        //            { "BatchNo", request.BatchNo ?? (object)DBNull.Value },
        //            { "FarmerName", request.FarmerName ?? (object)DBNull.Value },
        //            { "Mobile", request.Mobile ?? (object)DBNull.Value },
        //            { "InsertMode", request.InsertMode ?? "IMP" },
        //            { "Status", request.Status ?? "PENDING" },
        //            { "Shift", request.Shift ?? (object)DBNull.Value },
        //            { "MppID", request.MppID },
        //            { "BmcID", request.BmcID },
        //            { "RefranceId", request.RefranceId ?? (object)DBNull.Value },
        //            { "Can", request.Can ?? (object)DBNull.Value },
        //            { "IsValidated", request.IsValidated },
        //            { "IsProcess", request.IsProcess },
        //            { "CId", request.CId },
        //            { "ProcessDate", request.ProcessDate ?? (object)DBNull.Value },
        //            //{ "CompanyID", request.CompanyCode ?? (object)DBNull.Value },
        //            { "IMEI_No", request.IMEINo ?? (object)DBNull.Value },
        //            { "is_status", request.IsStatus ?? (object)DBNull.Value },
        //            // System fields for update
        //            { "ModifiedBy", request.ModifiedBy ?? 0 },
        //            { "BusinessId", request.BusinessId  },
        //        };

        //        var message = await repository.UpdateAsync(FarmerStgQueries.AddFarmerStg, parameters, CommandType.StoredProcedure);
        //        if (message.StartsWith("Error"))
        //        {
        //            throw new Exception($"Stored Procedure Error: {message}");
        //        }

        //        logging.LogInfo($"FarmerStg with ID {request.CollecionID} updated successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        logging.LogError($"Error updating FarmerCollStg record with ID: {request.CollecionID}", ex);
        //        throw;
        //    }
        //}


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
                { "HeaderId", id }
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
            var parameters = new Dictionary<string, object>
            {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<FarmerCollResponse, int, FiltersMeta>(
                    FarmerStgQueries.GetFarmerStgList,
                    DbConstants.Main,
                    parameters,
                    null);

            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            var filteredCount = filtered.Count();

            //var list = JsonSerializer.Deserialize<List<FarmerCollectionStagingDetail>>();

            return new ListsResponse<FarmerCollResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

      

      
    }
}
