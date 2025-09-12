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
using MilkMatrix.Milk.Contracts.Inventory.ItemCategory;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Inventory.ItemCategory;
using MilkMatrix.Milk.Models.Response.Inventory.ItemCategory;

namespace MilkMatrix.Milk.Implementations
{
    public class ItemCatgService : IItemCatgService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public ItemCatgService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(ItemCatgService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddItemCatg(ItemCatgInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "Business_Id", request.BusinessId },
                    { "Parent_Id", request.ParentId ?? (object)DBNull.Value },
                    { "Code", request.Code },
                    { "Name", request.Name },
                    { "Is_Active", request.IsActive ?? true },
                    { "Created_By", request.CreatedBy ?? 0 }
                };

                var message = await repository.AddAsync(ItemCatgQueries.AddItemCatg, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                logging.LogInfo($"ItemCatg {message} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddItemCatg: {request.Name}", ex);
                throw;
            }
        }

        public async Task UpdateItemCatg(ItemCatgUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "Id", request.Id },
                    { "Business_Id", request.BusinessId },
                    { "Parent_Id", request.ParentId ?? (object)DBNull.Value },
                    { "Code", request.Code },
                    { "Name", request.Name },
                    { "Is_Active", request.IsActive ?? true },
                    { "Modify_By", request.ModifyBy ?? 0 }
                };

                var message = await repository.UpdateAsync(ItemCatgQueries.AddItemCatg, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                logging.LogInfo($"ItemCatg {request.Name} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateItemCatg: {request.Name}", ex);
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
                    { "Id", id }
                };

                var message = await repository.DeleteAsync(ItemCatgQueries.AddItemCatg, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                logging.LogInfo($"ItemCatg with id {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for ItemCatg id: {id}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<ItemCatgResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<ItemCatgResponse, int, FiltersMeta>(ItemCatgQueries.GetCatgList,
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

            return new ListsResponse<ItemCatgResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<ItemCatgResponse?> GetById(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for ItemCatg id: {id}");
                var repo = repositoryFactory.ConnectDapper<ItemCatgResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<ItemCatgResponse>(ItemCatgQueries.GetCatgList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "Id", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new ItemCatgResponse();
                logging.LogInfo(result != null
                    ? $"ItemCatg with id {id} retrieved successfully."
                    : $"ItemCatg with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for ItemCatg id: {id}", ex);
                throw;
            }
        }

        public Task<IEnumerable<ItemCatgResponse>> GetItemCatg(ItemCatgRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
