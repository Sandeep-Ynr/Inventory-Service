using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MilkMatrix.Api.Models.Request.Milk.Transaction.FarmerStagingCollection;
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
using MilkMatrix.Milk.Contracts.Inventory.Item;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Inventory.Item;
using MilkMatrix.Milk.Models.Response.Inventory.Item;
using MilkMatrix.Milk.Models.Response.Price;
using static MilkMatrix.Milk.Models.Queries.MilkQueries;
using static MilkMatrix.Milk.Models.Queries.PriceQueries;

namespace MilkMatrix.Milk.Implementations.Inventory.Item
{
    public class ItemService : IItemService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public ItemService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(ItemService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddItem(ItemInsertRequest request)
        {
            try
            {
                // Serialize request into JSON (like FarmerCollectionStaging)
                string json = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });

                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "@ItemJSON", json }
                };

                var message = await repository.AddAsync(ItemQueries.AddItem, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Item {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error occurred while adding Item: {request.ItemName}", ex);
                throw;
            }
        }


        public async Task UpdateItem(ItemUpdateRequest request)
        {
            try
            {
                // Serialize the request into JSON
                string json = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });

                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "@ItemJSON", json }
                };

                var message = await repository.UpdateAsync(ItemQueries.AddItem, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }

                logging.LogInfo($"Item {request.ItemName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error occurred while updating Item: {request.ItemName}. Exception: {ex.Message}", ex);
                throw;
            }
        }

        public async Task Delete(long id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "@ItemJSON", null },
                    { "ItemId", id }
                };


                var message = await repository.DeleteAsync(
                    ItemQueries.AddItem,
                    requestParams,
                    CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");
                else
                    logging.LogInfo($"Item with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Item ID: {id}", ex);
                throw;
            }
        }


        public async Task<IListsResponse<ItemListResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<ItemListResponse, int, FiltersMeta>(ItemQueries.GetItemList,
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

            return new ListsResponse<ItemListResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }



        public async Task<ItemResponse?> GetById(long id)
        {
            try
            {
                var repo = repositoryFactory.ConnectDapper<ItemResp>(DbConstants.Main);

                var data = await repo.QueryAsync<ItemResp>(
                    ItemQueries.GetItemList, // make sure your SP supports Individual fetch
                     new Dictionary<string, object>
                     {
                        { "ActionType", (int)ReadActionType.Individual },
                        { "ItemId ", id }
                     },
                     null
                 );
              

                var record = data.FirstOrDefault();
                if (record == null) return null;

                // Deserialize JSON safely
                List<DairySpecResponse> dairySpecs = new();
                if (!string.IsNullOrEmpty(record.DairySpecs))
                {
                    try
                    {
                        dairySpecs = JsonSerializer.Deserialize<List<DairySpecResponse>>(record.DairySpecs) ?? new();
                    }
                    catch (JsonException jex)
                    {
                        logging.LogInfo("Invalid DairySpecs"  + jex);
                        
                    }
                }

                List<ItemLocationResponse> locations = new();
                if (!string.IsNullOrEmpty(record.Locations))
                {
                    try
                    {
                        locations = JsonSerializer.Deserialize<List<ItemLocationResponse>>(record.Locations) ?? new();
                    }
                    catch (JsonException jex)
                    {
                        logging.LogInfo("Invalid DairySpecs" + jex);
                    }
                }

                return new ItemResponse
                {
                    BusinessId = record.BusinessId,
                    ItemId = record.ItemId,
                    ItemTypeId = record.ItemTypeId,
                    LifecycleStatusId = record.LifecycleStatusId,
                    CategoryId = record.CategoryId,
                    SubCategoryId = record.SubCategoryId,
                    ItemCode = record.ItemCode,
                    ItemName = record.ItemName,
                    BaseUomId = record.BaseUomId,
                    IsPerishable = record.IsPerishable,
                    IsBatchTracked = record.IsBatchTracked,
                    IsSerialTracked = record.IsSerialTracked,
                    HsnSac = record.HsnSac,
                    Mrp = record.Mrp,
                    PurchaseRate = record.PurchaseRate,
                    SaleRate = record.SaleRate,
                    AvgRate = record.AvgRate,
                    Barcode = record.Barcode,
                    BrandId = record.BrandId,
                    Notes = record.Notes,
                    IsActive = record.IsActive,
                    CreatedBy = record.CreatedBy,
                    DairySpecs = dairySpecs,
                    Locations = locations
                };
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error fetching ItemId {ItemId}", itemId);
                throw;
            }
        }



    }
}
