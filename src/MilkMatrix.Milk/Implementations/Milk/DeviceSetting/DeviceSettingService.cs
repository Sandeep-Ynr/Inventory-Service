
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Request;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Contracts.Milk.DeviceSetting;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Milk.DeviceSetting;
using MilkMatrix.Milk.Models.Response.Bank;
using MilkMatrix.Milk.Models.Response.Milk.DeviceSetting;
using MilkMatrix.Milk.Models.Response.MPP;
using static MilkMatrix.Milk.Models.Queries.BankQueries;
using static MilkMatrix.Milk.Models.Queries.MilkQueries;

namespace MilkMatrix.Milk.Implementations.Milk.DeviceSetting
{
    public class DeviceSettingService : IDeviceSettingService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public DeviceSettingService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(DeviceSettingService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
            logging.LogInfo($"DeviceSettingService initialized with AppConfig: {appConfig.Value}");
        }

        public async Task DeleteDeviceSetting(int deviceSettingId, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType" , (int)CrudActionType.Delete },
                    {"DeviceSettingId", deviceSettingId },
                    {"ModifiedBy", userId }
                };
                var response = await repository.DeleteAsync(
                   DeviceSettingQueries.AddDeviceSetting, requestParams, CommandType.StoredProcedure
                );

                logging.LogInfo($"Bank Type with id {deviceSettingId} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Bank Type id: {deviceSettingId}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<DeviceSettingResponse>> GetAll(ListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<DeviceSettingResponse, int, FiltersMeta>(DeviceSettingQueries.GetDeviceSettingList,
                    DbConstants.Main,
                    parameters,
                    null);

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
            return new ListsResponse<DeviceSettingResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
        public async Task<DeviceSettingResponse?> GetDeviceSettingById(int deviceSettingId)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for Bank Type id: {deviceSettingId}");
                var repo = repositoryFactory
                           .ConnectDapper<DeviceSettingResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<DeviceSettingResponse>(DeviceSettingQueries.GetDeviceSettingList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "DeviceSettingId", deviceSettingId }
                }, null);

                var result = data.FirstOrDefault();

                logging.LogInfo(result != null
                    ? $"DeviceSetting with id {deviceSettingId} retrieved successfully."
                    : $"DeviceSetting with id {deviceSettingId} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for DeviceSettingId: {deviceSettingId}", ex);
                throw;
            }
        }

        public async Task InsertDeviceSetting(DeviceSettingInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "MppId", request.MppId },
                    { "EffectiveDate", request.EffectiveDate },
                    { "EffectiveShift", request.EffectiveShift ?? (object)DBNull.Value },
                    { "IsManual", request.IsManual },
                    { "EncryptUsbData", request.EncryptUsbData },
                    { "DpuModel", request.DpuModel ?? (object)DBNull.Value },
                    { "MaxCollectionPerShift", request.MaxCollectionPerShift },
                    { "IsWifiEnabled", request.IsWifiEnabled },
                    { "ApName", request.ApName ?? (object)DBNull.Value },
                    { "ApPassword", request.ApPassword ?? (object)DBNull.Value },
                    { "AdminPassword", request.AdminPassword ?? (object)DBNull.Value },
                    { "SupportPassword", request.SupportPassword ?? (object)DBNull.Value },
                    { "UserPassword", request.UserPassword ?? (object)DBNull.Value },
                    { "Apn", request.Apn ?? (object)DBNull.Value },
                    { "IsDispatchMandate", request.IsDispatchMandate },
                    { "IsMaCalibration", request.IsMaCalibration },
                    { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                    { "BusinessId ", request.BusinessId  ?? (object)DBNull.Value },
                };

                var message = await repository.AddAsync(DeviceSettingQueries.AddDeviceSetting, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"MPP {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddMPP: {request.ApName}", ex);
                throw;
            }
        }

        public async Task UpdateDeviceSetting(DeviceSettingUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "DeviceSettingId", request.DeviceSettingId },
                    { "MppId", request.MppId },
                    { "EffectiveDate", request.EffectiveDate },
                    { "EffectiveShift", request.EffectiveShift ?? (object)DBNull.Value },
                    { "IsManual", request.IsManual },
                    { "EncryptUsbData", request.EncryptUsbData },
                    { "DpuModel", request.DpuModel ?? (object)DBNull.Value },
                    { "MaxCollectionPerShift", request.MaxCollectionPerShift },
                    { "IsWifiEnabled", request.IsWifiEnabled },
                    { "ApName", request.ApName ?? (object)DBNull.Value },
                    { "ApPassword", request.ApPassword ?? (object)DBNull.Value },
                    { "AdminPassword", request.AdminPassword ?? (object)DBNull.Value },
                    { "SupportPassword", request.SupportPassword ?? (object)DBNull.Value },
                    { "UserPassword", request.UserPassword ?? (object)DBNull.Value },
                    { "Apn", request.Apn ?? (object)DBNull.Value },
                    { "IsDispatchMandate", request.IsDispatchMandate },
                    { "IsMaCalibration", request.IsMaCalibration },
                    { "IsStatus", request.IsActive ?? (object)DBNull.Value },
                    { "ModifiedBy", request.ModifiedBy ?? 0 },
                    { "BusinessId", "1" }
                };
                var message = await repository.UpdateAsync(DeviceSettingQueries.AddDeviceSetting, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Device Setting for MPP {request.MppId} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateDeviceSetting: MppId={request.MppId}, DeviceSettingId={request.DeviceSettingId}", ex);
                throw;
            }
        }

       


    }
}
