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
                    { "RowId", request.RowId },
                    { "BusinessEntityId", request.BusinessEntityId ?? (object)DBNull.Value},
                    { "PlantId", request.PlantId },
                    { "MccId", request.MccId },
                    { "BMC_Other_Code", request.BMC_Other_Code ?? (object)DBNull.Value },
                    { "MPP_Other_Code", request.MPP_Other_Code ?? (object)DBNull.Value },
                    { "CntCode", request.CntCode },
                    { "SocCode", request.SocCode },
                    { "RouteId", request.RouteId },
                    { "Shift", request.Shift ?? (object)DBNull.Value },
                    { "DispatchDate", request.DispatchDate },
                    { "DispatchTime", request.DispatchTime },
                    { "TotalSamples", request.TotalSamples },
                    { "Type", request.Type ?? (object)DBNull.Value },
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
                    { "Created_By", request.Created_By ?? (object)DBNull.Value },
                    { "Created_On", request.Created_On ?? (object)DBNull.Value },
                    { "Modify_By", request.Modify_By ?? (object)DBNull.Value },
                    { "Modify_On", request.Modify_On ?? (object)DBNull.Value }
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
                    { "Shift", request.Shift ?? (object)DBNull.Value },
                    { "DispatchDate", request.DispatchDate },
                    { "DispatchTime", request.DispatchTime },
                    { "TotalSamples", request.TotalSamples },
                    { "Type", request.Type ?? (object)DBNull.Value },
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
                    { "Created_By", request.Created_By ?? (object)DBNull.Value },
                    { "Created_On", request.Created_On ?? (object)DBNull.Value },
                    { "Modify_By", request.Modify_By ?? (object)DBNull.Value },
                    { "Modify_On", request.Modify_On ?? (object)DBNull.Value }
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
                    { "DispatchId", rowId }
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
                        { "DispatchId", rowId}
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
