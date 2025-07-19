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
using MilkMatrix.Milk.Contracts;
using MilkMatrix.Milk.Contracts.Logistics.Route;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Logistics.VehcileType;
using MilkMatrix.Milk.Models.Response.Logistics.VehicleType;
using static MilkMatrix.Milk.Models.Queries.VehicleQueries;

namespace MilkMatrix.Milk.Implementations.Logistics.Route
{
    public class VehicleTypeService : IVehicleTypeService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public VehicleTypeService(
            ILogging logging,
            IOptions<AppConfig> appConfig,
            IRepositoryFactory repositoryFactory,
            IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(VehicleTypeService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }
        public async Task<VehicleTypeResponse?> GetById(int vehicleId)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Vehicle ID: {vehicleId}");
                var repo = repositoryFactory.ConnectDapper<VehicleTypeResponse>(DbConstants.Main);

                var data = await repo.QueryAsync<VehicleTypeResponse>(GetVehicleList,
                    new Dictionary<string, object>
                    {
                        { "ActionType", (int)ReadActionType.Individual },
                        { "VehicleID", vehicleId }
                    }, null);

                var result = data.FirstOrDefault() ?? new VehicleTypeResponse();
                logging.LogInfo(result != null
                    ? $"Vehicle with ID {vehicleId} retrieved successfully."
                    : $"Vehicle with ID {vehicleId} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Vehicle ID: {vehicleId}", ex);
                throw;
            }
        }


        public async Task<IEnumerable<CommonLists>> GetSpecificLists(VehicleTypeRequest request)
        {
            var repository = repositoryFactory.Connect<VehicleTypeResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)(request.ActionType ?? ReadActionType.All) },
                { "IsStatus", request.IsStatus }
            };

            return await repository.QueryAsync<CommonLists>(GetVehicleList, requestParams, null, CommandType.StoredProcedure);
        }

        public async Task<IListsResponse<VehicleTypeResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<VehicleTypeResponse, int, FiltersMeta>(
                    GetVehicleList,
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

            return new ListsResponse<VehicleTypeResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task AddVehicleType(VehicleTypeInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "VehicleType", request.VehicleType },
                    { "Capacity", request.Capacity },
                    { "Description", request.Description ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus },
                    { "CreatedBy", request.CreatedBy }
                };

                await repository.AddAsync(AddVehicle, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Vehicle '{request.VehicleType}' added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddVehicle: {request.VehicleType}", ex);
                throw;
            }
        }

        public async Task UpdateVehicleType(VehicleTypeUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "VehicleID", request.VehicleID },
                    { "VehicleType", request.VehicleType },
                    { "Capacity", request.Capacity },
                    { "Description", request.Description ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus },
                    { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value }
                };

                await repository.UpdateAsync(AddVehicle, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Vehicle '{request.VehicleType}' updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateVehicle: {request.VehicleType}", ex);
                throw;
            }
        }

        public async Task Delete(int vehicleId, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "VehicleID", vehicleId },
                    { "ModifyBy", userId }
                };

                await repository.DeleteAsync(AddVehicle, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Vehicle with ID {vehicleId} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteVehicle for ID: {vehicleId}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<VehicleTypeResponse>> GetVehicleTypes(VehicleTypeRequest request)
        {
            var repository = repositoryFactory.Connect<VehicleTypeResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                { "VehicleID", request.VehicleID ?? 0 },
                { "ActionType", (int)(request.ActionType ?? ReadActionType.All) },
                { "IsStatus", request.IsStatus }
            };

            return await repository.QueryAsync<VehicleTypeResponse>(GetVehicleList, requestParams, null, CommandType.StoredProcedure);
        }

    
    }
}
