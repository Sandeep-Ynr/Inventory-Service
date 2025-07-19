//using System.Data;
//using Microsoft.Extensions.Options;
//using MilkMatrix.Core.Abstractions.DataProvider;
//using MilkMatrix.Core.Abstractions.Listings.Request;
//using MilkMatrix.Core.Abstractions.Listings.Response;
//using MilkMatrix.Core.Abstractions.Logger;
//using MilkMatrix.Core.Abstractions.Repository.Factories;
//using MilkMatrix.Core.Entities.Config;
//using MilkMatrix.Core.Entities.Enums;
//using MilkMatrix.Core.Entities.Filters;
//using MilkMatrix.Core.Entities.Response;
//using MilkMatrix.Core.Extensions;
//using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
//using MilkMatrix.Milk.Contracts.Logistics.Vehicle;
//using MilkMatrix.Milk.Models.Queries;
//using MilkMatrix.Milk.Models.Request.Logistics.Vehicle;
//using MilkMatrix.Milk.Models.Response.Logistics.Vehicle;

//namespace MilkMatrix.Milk.Implementations
//{
//    public class VehicleService : IVehicleService
//    {
//        private readonly ILogging logging;
//        private readonly AppConfig appConfig;
//        private readonly IRepositoryFactory repositoryFactory;
//        private readonly IQueryMultipleData queryMultipleData;

//        public VehicleService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
//        {
//            this.logging = logging.ForContext("ServiceName", nameof(VehicleService));
//            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
//            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
//            this.queryMultipleData = queryMultipleData;
//        }

//        public async Task AddVehicle(VehicleInsertRequest request)
//        {
//            try
//            {
//                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
//                var requestParams = new Dictionary<string, object>
//                {
//                    { "ActionType", (int)CrudActionType.Create },
//                    { "TransporterID", request.TransporterID },
//                    { "VehicleNo", request.VehicleTypeId },
//                    { "RCNo", request.RCBookNo ?? (object)DBNull.Value },
//                    { "ChassisNo", request.ChassisNo ?? (object)DBNull.Value },
//                    { "InsuranceNo", request.InsuranceNo ?? (object)DBNull.Value },
//                    { "InsuranceDate", request.InsuranceDate ?? (object)DBNull.Value },
//                    { "PollutionNo", request.PollutionNo ?? (object)DBNull.Value },
//                    { "PollutionDate", request.PollutionDate ?? (object)DBNull.Value },
//                    { "DriverName", request.DriverName ?? (object)DBNull.Value },
//                    { "MobileNo", request.MobileNo ?? (object)DBNull.Value },
//                    { "CompanyCode", request.CompanyCode },
//                    { "IsActive", request.IsActive },
//                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value }
//                };
//                var message = await repository.AddAsync(VehicleQueries.AddVehicle, requestParams, CommandType.StoredProcedure);
//                if (message.StartsWith("Error"))
//                    throw new Exception($"Stored Procedure Error: {message}");

//                logging.LogInfo($"Vehicle {request.VehicleNo} added successfully with response: {message}");
//            }
//            catch (Exception ex)
//            {
//                logging.LogError($"Error in AddVehicle: {request.VehicleNo}", ex);
//                throw;
//            }
//        }

//        public async Task UpdateVehicle(VehicleUpdateRequest request)
//        {
//            try
//            {
//                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
//                var requestParams = new Dictionary<string, object>
//                {
//                    { "ActionType", (int)CrudActionType.Update },
//                    { "VehicleID", request.VehicleID },
//                    { "TransporterID", request.TransporterID },
//                    { "VehicleNo", request.VehicleNo },
//                    { "RCNo", request.RCNo ?? (object)DBNull.Value },
//                    { "ChassisNo", request.ChassisNo ?? (object)DBNull.Value },
//                    { "InsuranceNo", request.InsuranceNo ?? (object)DBNull.Value },
//                    { "InsuranceDate", request.InsuranceDate ?? (object)DBNull.Value },
//                    { "PollutionNo", request.PollutionNo ?? (object)DBNull.Value },
//                    { "PollutionDate", request.PollutionDate ?? (object)DBNull.Value },
//                    { "DriverName", request.DriverName ?? (object)DBNull.Value },
//                    { "MobileNo", request.MobileNo ?? (object)DBNull.Value },
//                    { "CompanyCode", request.CompanyCode },
//                    { "IsActive", request.IsActive },
//                    { "ModifiedBy", request.ModifiedBy ?? (object)DBNull.Value }
//                };

//                var message = await repository.UpdateAsync(VehicleQueries.AddVehicle, requestParams, CommandType.StoredProcedure);
//                if (message.StartsWith("Error"))
//                    throw new Exception($"Stored Procedure Error: {message}");

//                logging.LogInfo($"Vehicle {request.VehicleNo} updated successfully with response: {message}");
//            }
//            catch (Exception ex)
//            {
//                logging.LogError($"Error in UpdateVehicle: {request.VehicleNo}", ex);
//                throw;
//            }
//        }

//        public async Task Delete(int id, int userId)
//        {
//            try
//            {
//                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
//                var requestParams = new Dictionary<string, object>
//                {
//                    { "ActionType", (int)CrudActionType.Delete },
//                    { "VehicleID", id },
//                    { "ModifiedBy", userId },
//                };

//                var response = await repository.DeleteAsync(VehicleQueries.AddVehicle, requestParams, CommandType.StoredProcedure);
//                logging.LogInfo($"Vehicle with id {id} deleted successfully.");
//            }
//            catch (Exception ex)
//            {
//                logging.LogError($"Error in DeleteAsync for Vehicle id: {id}", ex);
//                throw;
//            }
//        }

//        public async Task<VehicleResponse?> GetById(int id)
//        {
//            try
//            {
//                logging.LogInfo($"GetById called for Vehicle ID: {id}");
//                var repo = repositoryFactory.ConnectDapper<VehicleResponse>(DbConstants.Main);
//                var data = await repo.QueryAsync<VehicleResponse>(VehicleQueries.GetVehicleList, new Dictionary<string, object>
//                {
//                    { "ActionType", (int)ReadActionType.Individual },
//                    { "VehicleID", id }
//                }, null);

//                return data.FirstOrDefault();
//            }
//            catch (Exception ex)
//            {
//                logging.LogError($"Error in GetById for Vehicle ID: {id}", ex);
//                throw;
//            }
//        }

//        public async Task<IEnumerable<VehicleResponse>> GetVehicles(VehicleRequest request)
//        {
//            try
//            {
//                var repo = repositoryFactory.ConnectDapper<VehicleResponse>(DbConstants.Main);
//                var parameters = new Dictionary<string, object>
//                {
//                    { "ActionType", (int)(request.ActionType ?? ReadActionType.All) },
//                    { "VehicleID", request.VehicleID ?? (object)DBNull.Value },
//                    { "IsActive", request.IsStatus }
//                };

//                return await repo.QueryAsync<VehicleResponse>(VehicleQueries.GetVehicleList, parameters, null);
//            }
//            catch (Exception ex)
//            {
//                logging.LogError("Error in GetVehicles", ex);
//                throw;
//            }
//        }

//        public async Task<IListsResponse<VehicleResponse>> GetAll(IListsRequest request)
//        {
//            var parameters = new Dictionary<string, object> {
//                { "ActionType", (int)ReadActionType.All }
//            };

//            var (allResults, countResult, filterMetas) = await queryMultipleData
//                .GetMultiDetailsAsync<VehicleResponse, int, FiltersMeta>(VehicleQueries.GetVehicleList,
//                    DbConstants.Main, parameters, null);

//            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
//            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
//            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

//            var filtered = allResults.AsQueryable().ApplyFilters(filters);
//            var sorted = filtered.ApplySorting(sorts);
//            var paged = sorted.ApplyPaging(paging);
//            var filteredCount = filtered.Count();

//            return new ListsResponse<VehicleResponse>
//            {
//                Count = filteredCount,
//                Results = paged.ToList(),
//                Filters = filterMetas
//            };
//        }

//        Task<VehicleResponse?> IVehicleService.GetById(int id)
//        {
//            throw new NotImplementedException();
//        }

//        Task<IEnumerable<VehicleResponse>> IVehicleService.GetVehicles(VehicleRequest request)
//        {
//            throw new NotImplementedException();
//        }

//        Task<IListsResponse<VehicleResponse>> IVehicleService.GetAll(IListsRequest request)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
