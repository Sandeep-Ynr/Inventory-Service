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
using MilkMatrix.Milk.Contracts.Plant;
using MilkMatrix.Milk.Models.Request.Geographical;

//using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Request.Plant;
using MilkMatrix.Milk.Models.Response.Geographical;
using MilkMatrix.Milk.Models.Response.Plant;
using static MilkMatrix.Milk.Models.Queries.GeographicalQueries;
using static MilkMatrix.Milk.Models.Queries.PlantQueries;

namespace MilkMatrix.Milk.Implementations.Plant
{
    public class PlantService : IPlantService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public PlantService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(PlantService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddPlantAsync(PlantInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                    { "PlantName", request.PlantName ?? (object)DBNull.Value },
                    { "CompanyId", request.CompanyId },
                    { "Capacity", request.Capacity ?? (object)DBNull.Value },
                    { "FsssiNumber", request.FSSSINumber ?? (object)DBNull.Value },
                    { "Description", request.Description ?? (object)DBNull.Value },
                    { "Address", request.Address ?? (object)DBNull.Value },
                    { "StateId", request.StateId },
                    { "DistrictId", request.DistrictId },
                    { "TehsilId", request.TehsilId },
                    { "VillageId", request.VillageId },
                    { "HamletId", request.HamletId },
                    { "Pincode", request.Pincode },
                    { "Latitude", request.Latitude },
                    { "Longitude", request.Longitude },
                    { "RegionalName", request.RegionalName ?? (object)DBNull.Value },
                    { "ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
                    { "RegionalContactPerson", request.RegionalContactPerson ?? (object)DBNull.Value },
                    { "MobileNo", request.MobileNo ?? (object)DBNull.Value },
                    { "EmailID", request.EmailId ?? (object)DBNull.Value },
                    { "StartDate", request.StartDate ?? (object)DBNull.Value },
                    { "IsWorking", request.IsWorking ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                };
                var response = await repository.AddAsync(PlantQuery.AddPlant, requestParams, CommandType.StoredProcedure);
                // Return the inserted StateId or Name, etc. depending on your SP response
                //return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
                logging.LogInfo($"District {request.PlantName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for District: {request.PlantName}", ex);
                throw;
            }
        }

        public async Task UpdatePlantAsync(PlantUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "PlantId", request.PlantId},
                    { "PlantName", request.PlantName ?? (object)DBNull.Value },
                    { "CompanyId", request.CompanyId},
                    { "Capacity", request.Capacity ?? (object)DBNull.Value },
                    { "FsssiNumber", request.FSSSINumber},
                    { "Description", request.Description ?? (object)DBNull.Value },
                    { "Address", request.Address ?? (object)DBNull.Value },
                    { "StateId", request.StateId},
                    { "DistrictId", request.DistrictId},
                    { "TehsilId", request.TehsilId},
                    { "VillageId", request.VillageId},
                    { "HamletId", request.HamletId},
                    { "Pincode", request.Pincode },
                    { "Latitude", request.Latitude },
                    { "Longitude", request.Longitude },
                    { "RegionalName", request.RegionalName ?? (object)DBNull.Value },
                    { "ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
                    { "RegionalContactPerson", request.RegionalContactPerson ?? (object)DBNull.Value },
                    { "MobileNo", request.MobileNo ?? (object)DBNull.Value },
                    { "EmailID", request.EmailId ?? (object)DBNull.Value },
                    { "StartDate", request.StartDate ?? (object)DBNull.Value },
                    { "IsWorking", request.IsWorking ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                await repository.UpdateAsync(PlantQuery.AddPlant, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"District {request.PlantName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Plant: {request.PlantName}", ex);
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
                    {"PlantId", id },
                    {"IsActive", false },
                    {"ModifyBy", userId }
                    
                };

                var response = await repository.DeleteAsync(PlantQuery.AddPlant, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Plant with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Plant id: {id}", ex);
                throw;
            }

        }

        public async Task<IListsResponse<PlantResponse>> GetAllAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<PlantResponse, int, FiltersMeta>(PlantQuery.GetPlantList,
                    DbConstants.Main, parameters, null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<PlantResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<PlantInsertResponse?> GetByIdAsync(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Plant id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<PlantInsertResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<PlantInsertResponse>(PlantQuery.GetPlantList, new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "PlantId", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new PlantInsertResponse();
                logging.LogInfo(result != null
                    ? $"Plant with id {id} retrieved successfully."
                    : $"Plant with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Plant id: {id}", ex);
                throw;
            }
        }


    }
}
