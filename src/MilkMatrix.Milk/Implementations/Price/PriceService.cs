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
    public class PriceService : IPriceService
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
                // Serialize request into JSON
                string json = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                // Parameters for stored procedure
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "HeaderJSON", json }
                };

                // Call SP
                var message = await repository.AddAsync(PriceQuery.InsupdMilkPrice, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logger.LogInfo($"Milk Price {message} added successfully.");
                }
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

                // Serialize request into JSON
                string json = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });

                // Build request parameters
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "HeaderJSON", json },
                    { "RateCode", request.RateCode }
                };

                // Call stored procedure
                var message = await repository.AddAsync(PriceQuery.InsupdMilkPrice, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logger.LogInfo($"Milk Price {message} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in UpdateAsync for Milk Price: {request.WithEffectDate}", ex);
                throw;
            }
        }


        public async Task DeleteAsync(string ratecode, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "HeaderJSON", null },
                    { "RateCode", ratecode }
                };
                var message = await repository.DeleteAsync(PriceQuery.InsupdMilkPrice, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logger.LogInfo($"Milk Price {message} updated successfully.");
                }


                logger.LogInfo($"Milk Price with ID {ratecode} deleted successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in DeleteAsync for Milk Price ID: {ratecode}", ex);
                throw;
            }
        }


        public async Task<IListsResponse<MilkPriceInsertResponse>> GetAllAsync(IListsRequest request)
        {
            // 1. Prepare parameters
            var parameters = new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.All },
                    { "Start", request.Limit },
                    { "End", request.Offset }
                };

            // 2. Fetch results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<MilkPriceInsertResponse, int, FiltersMeta>(
                    PriceQuery.MilkPriceList,
                    DbConstants.Main,
                    parameters,
                    null);

            // 3. Apply filters, sorting, and paging
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering
            var filteredCount = filtered.Count();

            // 5. Return response object
            return new ListsResponse<MilkPriceInsertResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }


        //public async Task<MilkPriceInsertResponse?> GetByIdAsync(int id)
        //{
        //    try
        //    {
        //        logger.LogInfo($"GetByIdAsync called for Milk Price id: {id}");
        //        var repo = repositoryFactory
        //                   .ConnectDapper<MilkPriceInsertResponse>(DbConstants.Main);
        //        var data = await repo.QueryAsync<MilkPriceInsertResponse>(PriceQuery.MilkPriceList, new Dictionary<string, object> {
        //            { "ActionType", (int)ReadActionType.Individual },
        //            { "RateCode", id }
        //        }, null);

        //        var result = data.Any() ? data.FirstOrDefault() : new MilkPriceInsertResponse();
        //        logger.LogInfo(result != null
        //            ? $"Milk Price with id {id} retrieved successfully."
        //            : $"Milk Price with id {id} not found.");
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError($"Error in GetByIdAsync for Milk Price id: {id}", ex);
        //        throw;
        //    }
        //}
        public Task<MilkPriceInsertResponse?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<MilkPriceInsertResponse?> GetById(int id)
        {
            var repo = repositoryFactory.ConnectDapper<MilkPriceInsertResponse>(DbConstants.Main);
            var data = await repo.QueryAsync<MilkPriceInsertResponse>(
                PriceQuery.MilkPriceList, // make sure your SP supports Individual fetch
                new Dictionary<string, object>
                {
                { "ActionType", (int)ReadActionType.Individual },
                { "RateCode", id }
                },
                null
            );

            var record = data.FirstOrDefault();
            if (record == null) return null;

            // Deserialize PriceDetails JSON inline
            var priceDetails = string.IsNullOrEmpty(record.PriceDetails)
                ? new List<MilkChartRow>()
                : JsonSerializer.Deserialize<List<MilkChartRow>>(record.PriceDetails);

            // Return clean object
            return new MilkPriceInsertResponse
            {
                Id = record.Id,
                BusinessEntityId = record.BusinessEntityId,
                WithEffectDate = record.WithEffectDate,
                ShiftId = record.ShiftId,
                MilkTypeId = record.MilkTypeId,
                RateTypeId = record.RateTypeId,
                Description = record.Description,
                RateGenType = record.RateGenType,
                IsActive = record.IsActive,
                CreatedBy = record.CreatedBy,
                PriceDetails = record.PriceDetails // keep raw JSON
                                                   // if you want deserialized details, extend the model like below 👇
            };
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
