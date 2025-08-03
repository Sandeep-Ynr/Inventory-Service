using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.Csv;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.HostedServices;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.Utils;
using MilkMatrix.Milk.Contracts.Price;
using MilkMatrix.Milk.Models.Request;
using MilkMatrix.Milk.Models.Request.Price;
using MilkMatrix.Milk.Models.Response;
using MilkMatrix.Milk.Models.Response.Price;

using static MilkMatrix.Milk.Models.Queries.PriceQueries;

namespace MilkMatrix.Milk.Implementations.Price
{
    public class PriceService :IPriceService
    {
        private ILogging logger;

        private readonly IRepositoryFactory repositoryFactory;

        private readonly ICsvReader csvReader;
        private readonly IQueryMultipleData queryMultipleData;

        private readonly FileConfig fileConfig;

        private readonly AppConfig appConfig;

        private readonly IBulkProcessingTasks bulkProcessingTasks;

        public PriceService(IBulkProcessingTasks bulkProcessingTasks, ILogging logger, IRepositoryFactory repositoryFactory, IOptions<FileConfig> fileConfig, IQueryMultipleData queryMultipleData, IOptions<AppConfig> appConfig, ICsvReader csvReader)
        {
            this.bulkProcessingTasks = bulkProcessingTasks;
            this.logger = logger.ForContext("ServiceName", nameof(PriceService));
            this.repositoryFactory = repositoryFactory;
            this.fileConfig = fileConfig.Value ?? throw new ArgumentNullException(nameof(fileConfig), "FileConfig cannot be null");
            this.queryMultipleData = queryMultipleData;
            
        }

        public async Task AddAsync(MilkPriceInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                    { "BusinessEntityId",request.BusinessEntityId ?? (object)DBNull.Value },
                    { "WithEffectDate", request.WithEffectDate ?? (object)DBNull.Value },
                    { "ShiftId", request.ShiftId ?? (object)DBNull.Value },
                    { "MilkTypeId", request.MilkTypeId ?? (object)DBNull.Value },
                    { "RateTypeId", request.RateTypeId ?? (object)DBNull.Value },
                    { "Description", request.Description ?? (object)DBNull.Value },
                    { "RateGenType", request.RateGenType ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                    //{"PriceDetail" , request.PriceDetails ?? (object)DBNull.Value }, 
                };
                var response = await repository.AddAsync(PriceQuery.InsupdMilkPrice, requestParams, CommandType.StoredProcedure);
                // Return the inserted StateId or Name, etc. depending on your SP response
                //return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";


                var milkPriceCode = Convert.ToInt32(response);

                // 2. Loop through details and insert one-by-one
                foreach (var detail in request.PriceDetails)
                {
                    var detailParams = new Dictionary<string, object>
                    {
                        { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                        { "RateCode", milkPriceCode },
                        { "FAT", detail.Fat ?? (object)DBNull.Value },
                        { "Price", detail.Price ?? (object)DBNull.Value },
                        { "SNF", detail.SNF ?? (object)DBNull.Value },
                        { "IsActive", request.IsActive ?? (object)DBNull.Value },
                        { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                    };
                      await repository.AddAsync(PriceQuery.InsupdMilkPriceDetail, detailParams, CommandType.StoredProcedure);
                    //await repository.AddAsync("InsertMilkPriceDetail", detailParams, CommandType.StoredProcedure);
                }

                logger.LogInfo($"Milk Price {request.WithEffectDate} added with {request.PriceDetails?.Count ?? 0} detail records.");



                logger.LogInfo($"Milk Price {request.WithEffectDate} added successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in AddAsync for Milk Price: {request.WithEffectDate}", ex);
                throw;
            }
        }

        public async Task UpdateAsync(MilkPriceUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "RateCode", request.RateCode},
                    { "BusinessEntityId",request.BusinessEntityId ?? (object)DBNull.Value },
                    { "WithEffectDate", request.WithEffectDate ?? (object)DBNull.Value },
                    { "ShiftId", request.ShiftId ?? (object)DBNull.Value },
                    { "MilkTypeId", request.MilkTypeId ?? (object)DBNull.Value },
                    { "RateTypeId", request.RateTypeId ?? (object)DBNull.Value },
                    { "Description", request.Description ?? (object)DBNull.Value },
                    { "RateGenType", request.RateGenType ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                await repository.UpdateAsync(PriceQuery.InsupdMilkPrice, requestParams, CommandType.StoredProcedure);

                logger.LogInfo($"Milk Price {request.WithEffectDate} updated successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in UpdateAsync for Milk Price: {request.WithEffectDate}", ex);
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
                    {"ActionType" , (int)CrudActionType.Delete },
                    {"RateCode", id },
                    {"IsActive", false },
                    {"ModifyBy", userId }

                };

                var response = await repository.DeleteAsync(PriceQuery.InsupdMilkPrice, requestParams, CommandType.StoredProcedure);

                logger.LogInfo($"Milk Price with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError($"Error in DeleteAsync for Milk Price id: {id}", ex);
                throw;
            }

        }

        public async Task<IListsResponse<MilkPriceInsertResponse>> GetAllAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All },
                { "Start", request.Limit },
                { "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<MilkPriceInsertResponse, int, FiltersMeta>(PriceQuery.MilkPriceList,
                    DbConstants.Main, parameters, null);

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
            return new ListsResponse<MilkPriceInsertResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<MilkPriceInsertResponse?> GetByIdAsync(int id)
        {
            try
            {
                logger.LogInfo($"GetByIdAsync called for Milk Price id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<MilkPriceInsertResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<MilkPriceInsertResponse>(PriceQuery.MilkPriceList, new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "RateCode", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new MilkPriceInsertResponse();
                logger.LogInfo(result != null
                    ? $"Milk Price with id {id} retrieved successfully."
                    : $"Milk Price with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in GetByIdAsync for Milk Price id: {id}", ex);
                throw;
            }
        }

        public async Task<object> GetMilkFatChartJsonAsync(int rateCode)
        {
            try
            {
                var repo = repositoryFactory.ConnectDapper<dynamic>(DbConstants.Main);

                var result = await repo.QueryAsync<dynamic>(
                    PriceQuery.MilkRateChart,
                    new Dictionary<string, object>
                    {
                { "ActionType", (int)ReadActionType.Individual },
                { "RateCode", rateCode }
                    },
                    null
                );

                // Convert dynamic result to DataTable
                var dataTable = new DataTable();
                bool columnsBuilt = false;

                foreach (var row in result)
                {
                    var dict = (IDictionary<string, object>)row;

                    if (!columnsBuilt)
                    {
                        foreach (var key in dict.Keys)
                        {
                            dataTable.Columns.Add(key);
                        }
                        columnsBuilt = true;
                    }

                    var dataRow = dataTable.NewRow();
                    foreach (var key in dict.Keys)
                    {
                        dataRow[key] = dict[key] ?? DBNull.Value;
                    }
                    dataTable.Rows.Add(dataRow);
                }

                // Now extract columns
                var columns = dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();

                // Extract rows
                var rows = new List<List<object>>();
                foreach (DataRow row in dataTable.Rows)
                {
                    var rowData = new List<object>();
                    foreach (var column in columns)
                    {
                        var value = row[column];
                        rowData.Add(value == DBNull.Value ? null : value);
                    }
                    rows.Add(rowData);
                }

                return new
                {
                    columns,
                    rows
                };
            }
            catch (Exception ex)
            {
                logger.LogError("Error creating milk fat chart JSON", ex);
                throw;
            }
        }

    }
}
