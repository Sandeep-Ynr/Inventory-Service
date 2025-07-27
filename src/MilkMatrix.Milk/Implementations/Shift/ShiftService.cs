using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Dtos;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.Shift;
using MilkMatrix.Milk.Implementations.Shift;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Shift;
using MilkMatrix.Milk.Models.Response.Shift;
using static MilkMatrix.Milk.Models.Queries.ShiftQueries;

namespace MilkMatrix.Milk.Implementations.Shift
{
    public class ShiftService : IShiftService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public ShiftService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(ShiftService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddAsync(ShiftInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                    { "ShiftCode", request.ShiftCode ?? (object)DBNull.Value },
                    { "ShiftName",request.ShiftName ?? (object)DBNull.Value },
                    { "ShiftTime",request.ShiftTime ?? (object)DBNull.Value },
                    { "Description", request. Description ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                };
                var response = await repository.AddAsync(ShiftQuery.InsupdShift, requestParams, CommandType.StoredProcedure);
                // Return the inserted StateId or Name, etc. depending on your SP response
                //return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
                logging.LogInfo($"Shift {request.ShiftName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Shift: {request.ShiftName}", ex);
                throw;
            }
        }

        public async Task UpdateAsync(ShiftUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "ShiftId", request.ShiftId},
                    { "ShiftCode", request.ShiftCode ?? (object)DBNull.Value },
                    { "ShiftName",request.ShiftName ?? (object)DBNull.Value },
                    { "ShiftTime",request.ShiftTime ?? (object)DBNull.Value },
                    { "Description", request. Description ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                await repository.UpdateAsync(ShiftQuery.InsupdShift, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Shift {request.ShiftName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Shift: {request.ShiftName}", ex);
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
                    {"ShiftId", id },
                    {"IsActive", false },
                    {"ModifyBy", userId }

                };

                var response = await repository.DeleteAsync(ShiftQuery.InsupdShift, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Shift with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Shift id: {id}", ex);
                throw;
            }

        }

        public async Task<IListsResponse<ShiftInsertResponse>> GetAllAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All },
                { "Start", request.Limit },
                { "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<ShiftInsertResponse, int, FiltersMeta>(ShiftQuery.GetShiftList,
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
            return new ListsResponse<ShiftInsertResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<ShiftInsertResponse?> GetByIdAsync(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Shift id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<ShiftInsertResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<ShiftInsertResponse>(ShiftQuery.GetShiftList, new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "ShiftId", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new ShiftInsertResponse();
                logging.LogInfo(result != null
                    ? $"Shift with id {id} retrieved successfully."
                    : $"Shift with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Shift id: {id}", ex);
                throw;
            }
        }

    }
}
