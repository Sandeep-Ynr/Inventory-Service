using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
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
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.Milk.DockData;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Milk.DockData;
using MilkMatrix.Milk.Models.Response.Milk.DockData;
using static MilkMatrix.Milk.Models.Queries.MilkQueries;

namespace MilkMatrix.Milk.Implementations.Milk.DockData
{
    public class DockDataService : IDockDataService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public DockDataService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(DockDataService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task InsertDockData(DockDataInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "BusinessId", request.BusinessId },
                    { "BmcId", request.BmcId ?? (object)DBNull.Value},
                    { "DumpDate", request.DumpDate },
                    { "Shift", request.Shift ?? (object)DBNull.Value},
                    { "UpdateStatus", request.UpdateStatus ?? (object)DBNull.Value},
                    { "UpdatedRecords", request.UpdatedRecords },
                    { "Remarks", request.Remarks ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus  ?? (object)DBNull.Value},
                    { "CreatedBy", request.CreatedBy ?? 0 }
                };

                var message = await repository.AddAsync(DockDataQueries.AddDockData, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }

                logging.LogInfo($"DockData inserted successfully: {message}");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in InsertDockData for BMC Code: {request.BmcId}", ex);
                throw;
            }
        }

        public async Task UpdateDockData(DockDataUpdateRequest request)
        {
          try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "DockDataUpdateId", request.DockDataUpdateId },
                    { "BmcCode", request.BmcId },
                    { "DumpDate", request.DumpDate },
                    { "Shift", request.Shift },
                    { "UpdateStatus", request.UpdateStatus },
                    { "UpdatedRecords", request.UpdatedRecords },
                    { "Remarks", request.Remarks ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus ?? (object)DBNull.Value},
                    { "ModifiedBy", request.ModifiedBy  }
                };

                var message = await repository.UpdateAsync(DockDataQueries.AddDockData, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }

                logging.LogInfo($"DockData with ID {request.DockDataUpdateId} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateDockData for ID: {request.DockDataUpdateId}", ex);
                throw;
            }
        }

        public async Task DockDataDelete(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "DockDataUpdateId", id },
                    { "ModifiedBy", userId }
                };

                var result = await repository.DeleteAsync(DockDataQueries.AddDockData, requestParams, CommandType.StoredProcedure);
                if (result.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {result}");
                }

                logging.LogInfo($"DockData with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteDockData for ID: {id}", ex);
                throw;
            }
        }

        public async Task<DockDataResponse?> GetById(int id)
        {
            try
            {
                logging.LogInfo($"GetById called for DockData ID: {id}");
                var repo = repositoryFactory.ConnectDapper<DockDataResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<DockDataResponse>(
                    DockDataQueries.GetDockDataList,
                    new Dictionary<string, object>
                    {
                        { "ActionType", (int)ReadActionType.Individual },
                        { "DockDataUpdateId", id }
                    },
                    null);

                var result = data.FirstOrDefault();
                logging.LogInfo(result != null
                    ? $"DockData with ID {id} retrieved successfully."
                    : $"DockData with ID {id} not found.");

                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetById for DockData ID: {id}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<DockDataResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<DockDataResponse, int, FiltersMeta>(
                    DockDataQueries.GetDockDataList,
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

            return new ListsResponse<DockDataResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }



        public Task<IEnumerable<DockDataResponse>> GetDockData(DockDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
