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
using static MilkMatrix.Milk.Models.Queries.MilkQueries;

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

                // Minimal JSON for delete (SP uses this for data)
                var json = JsonSerializer.Serialize(new
                {
                    Item_Id = id,
                    ModifyBy = userId
                });

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete }, // MUST be separate
                    { "@ItemJSON", json }  // JSON data as SP expects
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






        public async Task<IListsResponse<ItemResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<ItemResponse, int, FiltersMeta>(ItemQueries.GetItemList,
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

            return new ListsResponse<ItemResponse>
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
                logging.LogInfo($"GetByIdAsync called for Item id: {id}");
                var repo = repositoryFactory.ConnectDapper<ItemResponse>(DbConstants.Main);

                // Create minimal JSON for SP
                var json = JsonSerializer.Serialize(new
                {
                    Item_Id = id
                });

                var parameters = new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual }, // Separate ActionType
                    { "@ItemJSON", json } // JSON containing ID
                };

                var data = await repo.QueryAsync<ItemResponse>(
                    ItemQueries.GetItemList,
                    parameters,
                    null);

                var result = data.FirstOrDefault();
                logging.LogInfo(result != null
                    ? $"Item with id {id} retrieved successfully."
                    : $"Item with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Item id: {id}", ex);
                throw;
            }
        }



    }
}
