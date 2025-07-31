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
using MilkMatrix.Milk.Contracts.Logistics.VehicleBillingType;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Logistics.VehicleBillingType;
using MilkMatrix.Milk.Models.Response.Logistics.VehicleBillingType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkMatrix.Milk.Implementations.Logistics.VehicleBillingType
{
    public class VehicleBillingTypeService : IVehicleBillingTypeService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public VehicleBillingTypeService(
            ILogging logging,
            IOptions<AppConfig> appConfig,
            IRepositoryFactory repositoryFactory,
            IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(VehicleBillingTypeService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddVehicleBillingType(VehicleBillingTypeInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "type_id", request.TypeId },
                    { "vehicle_id", request.VehicleId },
                    { "billing_type_id", request.BillingTypeId },
                    { "wef_date", request.WefDate },
                    { "remarks", request.Remarks ?? (object)DBNull.Value },
                    { "business_id", request.BusinessId },
                    { "transporter_id", request.TransporterId },
                    { "is_status", request.IsStatus },
                    { "created_by", request.CreatedBy ?? (object)DBNull.Value },
                 };

                var result = await repository.AddAsync(VehicleBillingTypeQueries.VehicleBilling, parameters, CommandType.StoredProcedure);
                if (result.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {result}");
                }

                logging.LogInfo($"Vehicle Billing Type added successfully for Vehicle Code {request.BillingTypeId}. Response: {result}");
            }
            catch (Exception ex)
            {
                logging.LogError("Error in AddVehicleBillingType", ex);
                throw;
            }
        }

        public async Task UpdateVehicleBillingType(VehicleBillingTypeUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Update},
                    { "type_id", request.TypeId },
                    { "vehicle_id", request.VehicleId },
                    { "billing_type_id", request.BillingTypeId },
                    { "wef_date", request.WefDate },
                    { "remarks", request.Remarks ?? (object)DBNull.Value },
                    { "business_id", request.BusinessId },
                    { "transporter_id", request.TransporterId },
                    { "is_status", request.IsStatus },
                    { "modify_by", request.ModifyBy ?? (object)DBNull.Value },
                };

                var result = await repository.UpdateAsync(VehicleBillingTypeQueries.VehicleBilling, parameters, CommandType.StoredProcedure);
                if (result.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {result}");
                }

                logging.LogInfo($"Vehicle Billing Type with Code {request} updated successfully. Response: {result}");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateVehicleBillingType for Code {request}", ex);
                throw;
            }
        }

        public async Task DeleteVehicleBillingType(long id, string deletedBy)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "type_Id", id }
                };

                var result = await repository.DeleteAsync(VehicleBillingTypeQueries.VehicleBilling, parameters, CommandType.StoredProcedure);
                if (result.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {result}");
                }
                logging.LogInfo($"Vehicle Billing Type with Code {id} deleted successfully. Response: {result}");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteVehicleBillingType for Code {id}", ex);
                throw;
            }
        }

        public async Task<VehicleBillingTypeResponse?> GetVehicleBillingTypeById(long id)
        {
            try
            {
                var repo = repositoryFactory.ConnectDapper<VehicleBillingTypeResponse>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual},
                    { "type_Id", id }
                };

                var result = await repo.QueryAsync<VehicleBillingTypeResponse>(VehicleBillingTypeQueries.VehicleBillingList, parameters, null);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetVehicleBillingTypeById for ID {id}", ex);
                throw;
            }
        }
        public async Task<IListsResponse<VehicleBillingTypeResponse>> GetAllVehicleBillingTypes(IListsRequest request)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.All }
                };

                var (allResults, countResult, filterMetas) = await queryMultipleData
                    .GetMultiDetailsAsync<VehicleBillingTypeResponse, int, FiltersMeta>(
                        VehicleBillingTypeQueries.VehicleBillingList,
                        DbConstants.Main, parameters, null);

                var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
                var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
                var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

                var filtered = allResults.AsQueryable().ApplyFilters(filters);
                var sorted = filtered.ApplySorting(sorts);
                var paged = sorted.ApplyPaging(paging);
                var filteredCount = filtered.Count();

                return new ListsResponse<VehicleBillingTypeResponse>
                {
                    Count = filteredCount,
                    Results = paged.ToList(),
                    Filters = filterMetas
                };
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetAllVehicleBillingTypes", ex);
                throw;
            }
        }

    }
}

