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
        public async Task DeleteFarmerCollection(long id, long userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "CollectionId", id },
                    { "ModifyBy", userId }
                };

                await repository.DeleteAsync(FarmerStgQueries.AddFarmerStg, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"FarmerStg with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteFarmerCollection for ID: {id}", ex);
                throw;
            }
        }
        public async Task ImportFarmerCollection(FarmerCollStgInsertRequest request)
        {
            try
            {
                // Serialize request into JSON (like Party)
                string json = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });

                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "@JsonData", json }
                };

                var message = await repository.AddAsync(FarmerStgQueries.AddFarmerStg, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"FarmerStg {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error occurred while adding FarmerStg Exception: {ex.Message}", ex);
                throw;
            }
        }
        public async Task<FarmerCollResponse?> GetFarmerCollectionExportById(string batchno)
        {
            try
            {
                logging.LogInfo($"GetById called for FarmerCollection ID: {batchno}");
                var repo = repositoryFactory.ConnectDapper<FarmerstagingCollResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<FarmerstagingCollResponse>(
                    FarmerStgQueries.GetFarmerStgList,
                    new Dictionary<string, object>
                    {
                        { "ActionType", (int)ReadActionType.Individual },
                        { "BatchNumber",batchno }
                    },
                    null);

                var record = data.FirstOrDefault();
                if (record == null) return null;
                var ExportDetail = string.IsNullOrEmpty(record.ExportList)
                      ? new List<FarmerCollectionStagingDetailModel>()
                      : JsonSerializer.Deserialize<List<FarmerCollectionStagingDetailModel>>(record.ExportList);
                return new FarmerCollResponse
                {
                    ExportList = ExportDetail,
                };
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetById for FarmerCollection ID: {batchno}", ex);
                throw;
            }
        }
        public async Task<IListsResponse<FarmerstagingCollResponse>> GetFarmerCollectionExport(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>
            {
                 { "ActionType", (int)ReadActionType.All },
                 { "BatchNumber", "" }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<FarmerstagingCollResponse, int, FiltersMeta>(
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
            return new ListsResponse<FarmerstagingCollResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async  Task DeleteFarmerCollectionExportById(string BatchNo, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "BatchNumber",BatchNo }
                };
                var message = await repository.DeleteAsync(
                    FarmerStgQueries.AddFarmerStg,
                    requestParams,
                    CommandType.StoredProcedure);
                
                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");
                else
                    logging.LogInfo($"FarmerCollection with id  deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for FarmerCollection id:", ex);
                throw;
            }
        }
    }
}
