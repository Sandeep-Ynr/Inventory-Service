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
using MilkMatrix.Milk.Contracts.Inventory.Item;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Inventory.ItemBrand;
using MilkMatrix.Milk.Models.Response.Inventory.ItemBrand;

namespace MilkMatrix.Milk.Implementations
{
    public class ItemBrandService : IItemBrandService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public ItemBrandService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(ItemBrandService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddItemBrand(ItemBrandInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "Business_Id", request.BusinessId },
                    { "Name", request.Name ?? (object)DBNull.Value },
                    { "Description", request.Description },
                    { "Is_Active", request.IsActive  },
                    { "Created_By", request.CreatedBy ?? 0 }
                };

                var message = await repository.AddAsync(ItemBrand.InsUpdItem, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                logging.LogInfo($"ItemBrand {message} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddItemCatg: {request.Name}", ex);
                throw;
            }
        }

        public async Task UpdateItemBrand(ItemBrandUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "Brand_Id", request.BrandId},
                    { "Business_Id", request.BusinessId },
                    { "Name", request.Name ?? (object)DBNull.Value },
                    { "Description", request.Description },
                    { "Is_Active", request.IsActive },
                    { "Modify_By", request.ModifyBy ?? 0 }
                };

                var message = await repository.UpdateAsync(ItemBrand.InsUpdItem, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                logging.LogInfo($"Item Brand {request.Name} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateItem  Brand: {request.Name}", ex);
                throw;
            }
        }

        public async Task Delete(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "Brand_Id", id }
                };

                var message = await repository.DeleteAsync(ItemBrand.InsUpdItem, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                logging.LogInfo($"Item Brand with id {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Item Brand id: {id}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<ItemBrandResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<ItemBrandResponse, int, FiltersMeta>(ItemBrand.GetItemList,
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

            return new ListsResponse<ItemBrandResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<ItemBrandResponse?> GetById(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Item Brand id: {id}");
                var repo = repositoryFactory.ConnectDapper<ItemBrandResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<ItemBrandResponse>(ItemBrand.GetItemList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "Id", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new ItemBrandResponse();
                logging.LogInfo(result != null
                    ? $"Item Brand with id {id} retrieved successfully."
                    : $"Item Brand with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Item Brand id: {id}", ex);
                throw;
            }
        }


    }
}
