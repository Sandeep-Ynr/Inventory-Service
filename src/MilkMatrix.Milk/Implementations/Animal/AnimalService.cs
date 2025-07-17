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
using MilkMatrix.Milk.Contracts.Animal;
using MilkMatrix.Milk.Implementations.Bmc;
using MilkMatrix.Milk.Models.Request.Animal;
using MilkMatrix.Milk.Models.Response.Animal;
using static MilkMatrix.Milk.Models.Queries.AnimalQueries;

namespace MilkMatrix.Milk.Implementations.Animal
{
    public class AnimalService : IAnimalService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public AnimalService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(BmcService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }


        public async Task AddAsync(AnimalTypeInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                    { "AnimalTypeCode", request.AnimalTypeCode ?? (object)DBNull.Value },
                    { "AnimalTypeName",request.AnimalTypeName ?? (object)DBNull.Value },
                    { "Description", request. Description ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                };
                var response = await repository.AddAsync(AnimalQuery.InsupdAnimalType, requestParams, CommandType.StoredProcedure);
                // Return the inserted StateId or Name, etc. depending on your SP response
                //return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
                logging.LogInfo($"Animal Type {request.AnimalTypeName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Animal Type: {request.AnimalTypeName}", ex);
                throw;
            }
        }

        public async Task UpdateAsync(AnimalTypeUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "AnimalTypeId", request.AnimalTypeId},
                    { "AnimalTypeCode", request.AnimalTypeCode ?? (object)DBNull.Value },
                    { "AnimalTypeName",request.AnimalTypeName ?? (object)DBNull.Value },
                    { "Description", request. Description ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                await repository.UpdateAsync(AnimalQuery.InsupdAnimalType, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Animal Type {request.AnimalTypeName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Animal Type: {request.AnimalTypeName}", ex);
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
                    {"AnimalTypeId", id },
                    {"IsActive", false },
                    {"ModifyBy", userId }

                };

                var response = await repository.DeleteAsync(AnimalQuery.InsupdAnimalType, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"BMC with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for BMC id: {id}", ex);
                throw;
            }

        }

        public async Task<IListsResponse<AnimalTypeInsertResponse>> GetAllAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All },
                { "Start", request.Limit },
                { "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<AnimalTypeInsertResponse, int, FiltersMeta>(AnimalQuery.GetAnimalTypeList,
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
            return new ListsResponse<AnimalTypeInsertResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<AnimalTypeInsertResponse?> GetByIdAsync(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for BMC id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<AnimalTypeInsertResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<AnimalTypeInsertResponse>(AnimalQuery.GetAnimalTypeList, new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "AnimalTypeId", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new AnimalTypeInsertResponse();
                logging.LogInfo(result != null
                    ? $"BMC with id {id} retrieved successfully."
                    : $"BMC with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for BMC id: {id}", ex);
                throw;
            }
        }

    }
}
