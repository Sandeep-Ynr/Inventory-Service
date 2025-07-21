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
using MilkMatrix.Milk.Contracts.Logistics.Vehicle;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Logistics.Vehicle;
using MilkMatrix.Milk.Models.Response.Logistics.Vehicle;
using MilkMatrix.Milk.Models.Response.Logistics.VehicleType;

namespace MilkMatrix.Milk.Implementations
{
    public class VehicleService : IVehicleService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public VehicleService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(VehicleService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddVehicle(VehicleInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                        { "ActionType", (int)CrudActionType.Create },
                        { "VehicleTypeId", request.VehicleTypeId },
                        { "CapacityCode", request.CapacityCode  ?? (object)DBNull.Value},
                        { "RegistrationNo", request.RegistrationNo  ?? (object)DBNull.Value},
                        { "ApplicableRTO", request.ApplicableRTO  ?? (object)DBNull.Value},
                        { "DriverName", request.DriverName  ?? (object)DBNull.Value},
                        { "DriverContactNo", request.DriverContactNo  ?? (object)DBNull.Value},
                        { "WEFDate", request.WEFDate },
                        { "DrivingLicenseNumber", request.DrivingLicenseNumber  ?? (object)DBNull.Value},
                        { "LicenceExpiryDate", request.LicenceExpiryDate  ?? (object)DBNull.Value},
                        { "TransporterCode", request.TransporterCode  ?? (object)DBNull.Value},
                        { "MappedRoute", request.MappedRoute  ?? (object)DBNull.Value},
                        { "PollutionCertificate", request.PollutionCertificate  ?? (object)DBNull.Value},
                        { "Insurance", request.Insurance ?? (object)DBNull.Value },
                        { "RCBookNo", request.RCBookNo ?? (object)DBNull.Value },
                        { "ExpiryDate", request.ExpiryDate  ?? (object)DBNull.Value},
                        { "Rent", request.Rent ?? (object)DBNull.Value },
                        { "Average", request.Average ?? (object)DBNull.Value },
                        { "CompanyCode", request.CompanyCode  ?? (object)DBNull.Value},
                        { "FuelTypeCode", request.FuelTypeCode  ?? (object)DBNull.Value},
                        { "PassingNo", request.PassingNo  ?? (object)DBNull.Value},
                        { "BMCCode", request.BMCCode  ?? (object)DBNull.Value},
                        { "IsStatus", request.IsStatus },
                        { "created_by", request.CreatedBy },

                };
                var message = await repository.AddAsync(VehicleQueries.AddVehicle, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Vehicle {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddVehicle: {request.RegistrationNo}", ex);
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

                await repository.DeleteAsync(VehicleQueries.AddVehicle, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Vehicle with ID {vehicleId} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteVehicle for ID: {vehicleId}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<VehicleResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object> {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<VehicleResponse, int, FiltersMeta>(VehicleQueries.GetVehicleList,
                    DbConstants.Main, parameters, null);

            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);
            var filteredCount = filtered.Count();

            return new ListsResponse<VehicleResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<VehicleResponse?> GetById(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Vehicle ID: {id}");
                var repo = repositoryFactory.ConnectDapper<VehicleResponse>(DbConstants.Main);

                var data = await repo.QueryAsync<VehicleResponse>(VehicleQueries.GetVehicleList,
                    new Dictionary<string, object>
                    {
                        { "ActionType", (int)ReadActionType.Individual },
                        { "VehicleID", id }
                    }, null);

                var result = data.FirstOrDefault() ?? new VehicleResponse();
                logging.LogInfo(result != null
                    ? $"Vehicle with ID {id} retrieved successfully."
                    : $"Vehicle with ID {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Vehicle ID: {id}", ex);
                throw;
            }
        }

        public Task<IEnumerable<VehicleResponse>> GetVehicles(VehicleRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateVehicle(VehicleUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "VehicleID", request.VehicleId },
                    { "VehicleTypeId", request.VehicleTypeId },
                    { "CapacityCode", request.CapacityCode ?? (object)DBNull.Value },
                    { "RegistrationNo", request.RegistrationNo ?? (object)DBNull.Value },
                    { "ApplicableRTO", request.ApplicableRTO ?? (object)DBNull.Value },
                    { "DriverName", request.DriverName ?? (object)DBNull.Value },
                    { "DriverContactNo", request.DriverContactNo ?? (object)DBNull.Value },
                    { "WEFDate", request.WEFDate },
                    { "DrivingLicenseNumber", request.DrivingLicenseNumber ?? (object)DBNull.Value },
                    { "LicenceExpiryDate", request.LicenceExpiryDate ?? (object)DBNull.Value },
                    { "TransporterCode", request.TransporterCode ?? (object)DBNull.Value },
                    { "MappedRoute", request.MappedRoute ?? (object)DBNull.Value },
                    { "PollutionCertificate", request.PollutionCertificate ?? (object)DBNull.Value },
                    { "Insurance", request.Insurance ?? (object)DBNull.Value },
                    { "RCBookNo", request.RCBookNo ?? (object)DBNull.Value },
                    { "ExpiryDate", request.ExpiryDate ?? (object)DBNull.Value },
                    { "Rent", request.Rent ?? (object)DBNull.Value },
                    { "Average", request.Average ?? (object)DBNull.Value },
                    { "CompanyCode", request.CompanyCode ?? (object)DBNull.Value },
                    { "FuelTypeCode", request.FuelTypeCode ?? (object)DBNull.Value },
                    { "PassingNo", request.PassingNo ?? (object)DBNull.Value },
                    { "BMCCode", request.BMCCode ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus },
                    { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value }
                };

                var message = await repository.UpdateAsync(VehicleQueries.AddVehicle, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }

                logging.LogInfo($"Vehicle '{request.RegistrationNo}' updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateVehicle: {request.RegistrationNo}", ex);
                throw;
            }
        }

    }
}
