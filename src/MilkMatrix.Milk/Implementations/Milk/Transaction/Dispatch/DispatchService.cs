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
using MilkMatrix.Milk.Contracts.Milk.Transaction.Dispatch;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Milk.Transactions.Dispatch;
using MilkMatrix.Milk.Models.Response.Milk.Transaction.Dispatch;
using static MilkMatrix.Milk.Models.Queries.MilkQueries;


namespace MilkMatrix.Milk.Implementations.Milk.Transaction.Dispatch
{
    public class DispatchService : IDispatchService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public DispatchService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(DispatchService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }


        public async Task AddDispatch(DispatchInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                { "ActionType", (int)CrudActionType.Create },
                { "BusinessEntityId", request.BusinessEntityId ?? (object)DBNull.Value },
                { "plantId", request.PlantId },
                { "mccId", request.MccId },
                { "BMC_Other_Code", request.BMC_Other_Code ?? (object)DBNull.Value },
                { "MPP_Other_Code", request.MPP_Other_Code ?? (object)DBNull.Value },
                { "cntCode", request.CntCode },
                { "socCode", request.SocCode },
                { "routeId", request.RouteId },
                { "shiftId", request.ShiftId ?? (object)DBNull.Value },
                { "dispatchDate", request.DispatchDate },
                { "dispatchTime", request.DispatchTime },
                { "totalSamples", request.TotalSamples },
                { "typeId", request.TypeId ?? (object)DBNull.Value },
                { "grade", request.Grade ?? (object)DBNull.Value },
                { "weight", request.Weight },
                { "weightLiter", request.WeightLiter },
                { "fat", request.Fat },
                { "snf", request.Snf },
                { "lr", request.Lr },
                { "protein", request.Protein },
                { "water", request.Water },
                { "rtpl", request.Rtpl },
                { "totalAmount", request.TotalAmount },
                { "can", request.Can },
                { "isQtyAuto", request.IsQtyAuto },
                { "isQltyAuto", request.IsQltyAuto },
                { "qtytime", request.QtyTime },
                { "qltytime", request.QltyTime },
                { "kgLtrConst", request.KgLtrConst },
                { "ltrKgConst", request.LtrKgConst },
                { "qtyMode", request.QtyMode ?? (object)DBNull.Value },
                { "remark", request.Remark ?? (object)DBNull.Value },
                { "deviceId", request.DeviceId ?? (object)DBNull.Value },
                { "analyzerCode", request.AnalyzerCode },
                { "analyzerString", request.AnalyzerString ?? (object)DBNull.Value },
                { "cUserId", request.CUserId },
                { "mUserId", request.MUserId },
                { "MDateTime", request.MDateTime },
                { "isApproved", request.IsApproved },
                { "isRejected", request.IsRejected },
                { "PublicIp", request.PublicIp ?? (object)DBNull.Value },
                { "batch_id", request.Batch_Id ?? (object)DBNull.Value },
                { "insertMode", request.InsertMode ?? (object)DBNull.Value },
                { "is_status", request.Is_Status ?? (object)DBNull.Value },
                { "LastSynchronized", request.LastSynchronized },
                { "SyncStatus", request.SyncStatus ?? (object)DBNull.Value },
                { "SyncTime", request.SyncTime },
                { "created_by", request.Created_By ?? 0 },
                { "modify_by", request.Modify_By ?? (object)DBNull.Value }

                };

                var message = await repository.AddAsync(DispatchQueries.AddDispatch, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Dispatch {message} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddDispatch: {request.RowId}", ex);
                throw;
            }
        }

        public async Task UpdateDispatch(DispatchUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "RowId", request.RowId },
                    { "BusinessEntityId", request.BusinessEntityId?? (object)DBNull.Value },
                    { "PlantId", request.PlantId },
                    { "MccId", request.MccId },
                    { "BMC_Other_Code", request.BMC_Other_Code ?? (object)DBNull.Value },
                    { "MPP_Other_Code", request.MPP_Other_Code ?? (object)DBNull.Value },
                    { "CntCode", request.CntCode },
                    { "SocCode", request.SocCode },
                    { "RouteId", request.RouteId },
                    { "ShiftId", request.ShiftId ?? (object)DBNull.Value },
                    { "DispatchDate", request.DispatchDate },
                    { "DispatchTime", request.DispatchTime },
                    { "TotalSamples", request.TotalSamples },
                    { "TypeId", request.TypeId ?? (object)DBNull.Value },
                    { "Grade", request.Grade ?? (object)DBNull.Value },
                    { "Weight", request.Weight },
                    { "WeightLiter", request.WeightLiter },
                    { "Fat", request.Fat },
                    { "Snf", request.Snf },
                    { "Lr", request.Lr },
                    { "Protein", request.Protein },
                    { "Water", request.Water },
                    { "Rtpl", request.Rtpl },
                    { "TotalAmount", request.TotalAmount },
                    { "Can", request.Can },
                    { "IsQtyAuto", request.IsQtyAuto },
                    { "IsQltyAuto", request.IsQltyAuto },
                    { "QtyTime", request.QtyTime },
                    { "QltyTime", request.QltyTime },
                    { "KgLtrConst", request.KgLtrConst },
                    { "LtrKgConst", request.LtrKgConst },
                    { "QtyMode", request.QtyMode ?? (object)DBNull.Value },
                    { "Remark", request.Remark ?? (object)DBNull.Value },
                    { "DeviceId", request.DeviceId ?? (object)DBNull.Value },
                    { "AnalyzerCode", request.AnalyzerCode },
                    { "AnalyzerString", request.AnalyzerString ?? (object)DBNull.Value },
                    { "CUserId", request.CUserId },
                    { "CDateTime", request.CDateTime },
                    { "MUserId", request.MUserId },
                    { "MDateTime", request.MDateTime },
                    { "IsApproved", request.IsApproved },
                    { "IsRejected", request.IsRejected },
                    { "IsDelete", request.IsDelete },
                    { "PublicIp", request.PublicIp ?? (object)DBNull.Value },
                    { "LastSynchronized", request.LastSynchronized },
                    { "SyncStatus", request.SyncStatus ?? (object)DBNull.Value },
                    { "SyncTime", request.SyncTime },
                    { "Batch_Id", request.Batch_Id ?? (object)DBNull.Value },
                    { "InsertMode", request.InsertMode ?? (object)DBNull.Value },
                    { "Is_Status", request.Is_Status ?? (object)DBNull.Value },
                     { "modify_by", request.Modify_By ?? 0 }
                };

                var message = await repository.UpdateAsync(DispatchQueries.AddDispatch, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Dispatch {request.RowId} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateDispatch: {request.RowId}", ex);
                throw;
            }
        }

        public async Task Delete(decimal rowId, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "RowId", rowId }
                };

                var message = await repository.DeleteAsync(DispatchQueries.AddDispatch, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Dispatch with id {rowId} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Delete for Dispatch id: {rowId}", ex);
                throw;
            }
        }
 
        public async Task<IListsResponse<DispatchResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<DispatchResponse, int, FiltersMeta>(
                    DispatchQueries.GetDispatchList,
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

            return new ListsResponse<DispatchResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }


        public async Task<DispatchResponse?> GetById(decimal rowId)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Dispatch id: {rowId}");
                var repo = repositoryFactory.ConnectDapper<DispatchResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<DispatchResponse>(DispatchQueries.GetDispatchList,
                    new Dictionary<string, object>
                    {
                        { "ActionType", (int)ReadActionType.Individual },
                        { "RowId", rowId}
                    },
                    null);

                var result = data.Any() ? data.FirstOrDefault() : new DispatchResponse();
                logging.LogInfo(result != null
                    ? $"Dispatch with id {rowId} retrieved successfully."
                    : $"Dispatch with id {rowId} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetById for Dispatch id: {rowId}", ex);
                throw;
            }
        }
    }
}
