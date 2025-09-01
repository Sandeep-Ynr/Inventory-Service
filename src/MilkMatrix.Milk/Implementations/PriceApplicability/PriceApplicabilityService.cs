using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MilkMatrix.Api.Models.Request.PriceApplicability;
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
using MilkMatrix.Milk.Contracts.PriceApplicability;
using MilkMatrix.Milk.Models.Request.PriceApplicability;
using MilkMatrix.Milk.Models.Response.PriceApplicability;
using static MilkMatrix.Milk.Models.Queries.PriceApplicabilityQueries;

namespace MilkMatrix.Milk.Implementations.PriceApplicability
{
    public class PriceApplicabilityService : IPriceApplicabilityService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public PriceApplicabilityService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(PriceApplicabilityService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        private DataTable ConvertToDataTable(List<RateMappingTarget> targets)
        {
            var table = new DataTable();
            table.Columns.Add("plant_id", typeof(long));
            table.Columns.Add("mcc_id", typeof(long));
            table.Columns.Add("bmc_id", typeof(long));
            table.Columns.Add("route_id", typeof(long));
            table.Columns.Add("society_id", typeof(long));
            table.Columns.Add("farmer_id", typeof(long));
            table.Columns.Add("apply_to_all_below", typeof(bool));

            if (targets != null && targets.Any())
            {
                foreach (var t in targets)
                {
                    table.Rows.Add(
                        t.PlantId ?? (object)DBNull.Value,
                        t.MccId ?? (object)DBNull.Value,
                        t.BmcId ?? (object)DBNull.Value,
                        t.RouteId ?? (object)DBNull.Value,
                        t.SocietyId ?? (object)DBNull.Value,
                        t.FarmerId ?? (object)DBNull.Value,
                        t.ApplyToAllBelow
                    );
                }
            }

            return table;
        }

        public async Task AddAsync(PriceAppInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                    { "company_id",request.BusinessEntityId ?? (object)DBNull.Value },
                    { "with_effect_date", request.WithEffectDate ?? (object)DBNull.Value },
                    { "shift_id", request.ShiftId ?? (object)DBNull.Value },
                    { "applied_shift_scope", request.applied_shift_scope?? (object)DBNull.Value },
                    { "cattle_scope", request.cattleScope?? (object)DBNull.Value },
                    { "applied_for", request.applied_for?? (object)DBNull.Value },
                    { "priority", request.Priority  ?? (object)DBNull.Value},
                    { "notes", request.Description  ?? (object)DBNull.Value},
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                    { "rate_code_id", request.RateCodeId ?? (object)DBNull.Value },
                    { "targets", ConvertToDataTable(request.Targets)  },
                };
                var message = await repository.AddAsync(PriceApplicabilityQuery.InsupRateMapping, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Price Applicability: {request.ModuleName}");
                throw;
            }
        }

        public async Task UpdateAsync(PriceAppUpdateRequest request)
        {
            try
            {
                string base64 = request.RvOriginal;
                byte[] bytes = Convert.FromBase64String(base64);
                string hex = "0x" + BitConverter.ToString(bytes).Replace("-", "");
                string? hexValue = hex; // Example: "0x00000000000007D3"
                byte[]? rvOriginalBytes = null;

                if (!string.IsNullOrWhiteSpace(hexValue))
                {
                    // Remove "0x" prefix if present
                    if (hexValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    {
                        hexValue = hexValue.Substring(2);
                    }

                    // Convert to byte[]
                    rvOriginalBytes = Convert.FromHexString(hexValue);
                }
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "id",request.mappingid ?? (object)DBNull.Value },
                    { "company_id", request.BusinessEntityId ?? (object)DBNull.Value },
                    { "rate_code_id", request.RateCodeId ?? (object)DBNull.Value },
                    { "with_effect_date", request.WithEffectDate ?? (object)DBNull.Value },
                    { "shift_id", request.ShiftId ?? (object)DBNull.Value },
                    { "applied_shift_scope", request.applied_shift_scope ?? (object)DBNull.Value },
                    { "cattle_scope", request.cattleScope ?? (object)DBNull.Value },
                    { "applied_for", request.applied_for ?? (object)DBNull.Value },
                    { "priority", request.Priority ?? (object)DBNull.Value },
                    { "is_active", request.IsActive ?? (object)DBNull.Value},
                    { "notes", request.Description  ?? (object)DBNull.Value},
                    { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value },
                    { "rv_original", rvOriginalBytes ?? (object)DBNull.Value },
                    { "targets", ConvertToDataTable(request.Targets)  },
                };
                var message = await repository.AddAsync(PriceApplicabilityQuery.InsupRateMapping, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Price Applicability: {request.ModuleName}", ex);
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
                    {"id", id },
                    {"company_id", id },
                    {"is_active", false },
                    {"ModifyBy", userId }
                };
                var message = await repository.AddAsync(PriceApplicabilityQuery.InsupRateMapping, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }

                logging.LogInfo($"Price Applicability with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Price Applicability id: {id}", ex);
                throw;
            }

        }

        public async Task<IListsResponse<PriceAppInsertResponse>> GetAllAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All },
                { "Start", request.Limit },
                { "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<PriceAppInsertResponse, int, FiltersMeta>(PriceApplicabilityQuery.GetPriceAppList,
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
            return new ListsResponse<PriceAppInsertResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<PriceAppInsertResponse?> GetByIdAsync(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for BMC id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<PriceAppInsertResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<PriceAppInsertResponse>(PriceApplicabilityQuery.GetPriceAppList, new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "@mapping_id", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new PriceAppInsertResponse();
                logging.LogInfo(result != null
                    ? $"Price Applicability with id {id} retrieved successfully."
                    : $"Price Applicability with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for Price Applicability id: {id}", ex);
                throw;
            }
        }


        public async Task AddRateForAsync(RateForInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                    { "RateForCode", request.RateForCode ?? (object)DBNull.Value },
                    { "RateForName",request.RateForName ?? (object)DBNull.Value },
                    { "Description", request. Description ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                };
                var response = await repository.AddAsync(RateForQuery.AddRateFor, requestParams, CommandType.StoredProcedure);
                // Return the inserted StateId or Name, etc. depending on your SP response
                //return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
                logging.LogInfo($"Rate For {request.RateForName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for Rate For: {request.RateForName}", ex);
                throw;
            }
        }

        public async Task UpdateRateForAsync(RateForUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "RateForId", request.RateForId},
                    { "RateForCode", request.RateForCode ?? (object)DBNull.Value },
                    { "RateForName",request.RateForName ?? (object)DBNull.Value },
                    { "Description", request. Description ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                await repository.UpdateAsync(RateForQuery.AddRateFor, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Rate For {request.RateForName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Rate For: {request.RateForName}", ex);
                throw;
            }
        }

        public async Task DeleteRateForAsync(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType" , (int)CrudActionType.Delete },
                    {"RateForId", id },
                    {"IsActive", false },
                    {"ModifyBy", userId }

                };

                var response = await repository.DeleteAsync(RateForQuery.AddRateFor, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Rate For with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Rate For id: {id}", ex);
                throw;
            }

        }

        public async Task<IListsResponse<RateForInsertResponse>> GetAllRateForAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All },
                { "Start", request.Limit },
                { "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<RateForInsertResponse, int, FiltersMeta>(RateForQuery.GetRateForList,
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
            return new ListsResponse<RateForInsertResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<PriceActualRateResponse?> GetRateForByIdAsync(PriceAppRateforRequest request)
        {
            try
            {
                var repo = repositoryFactory.ConnectDapper<PriceActualRateResponse>(DbConstants.Main);

                var parameters = new Dictionary<string, object>
                {
                    { "company_id", request.CompanyId },
                    { "tx_date", request.TxDate },
                    { "shift_id", request.ShiftId },
                    { "cattle_scope", request.CattleScope },
                    { "plant_id", request.PlantId },
                    { "farmer_id", request.FarmerId },
                    { "level", request.Level },
                    { "view_type", request.ViewType },
                    { "fat", request.Fat },
                    { "snf", request.Snf },
                    { "society_id", request.SocietyId ?? (object)DBNull.Value },
                    { "route_id",   request.RouteId   ?? (object)DBNull.Value },
                    { "bmc_id",     request.BmcId     ?? (object)DBNull.Value },
                    { "mcc_id",     request.MccId     ?? (object)DBNull.Value }
                };


                var data = await repo.QueryAsync<PriceActualRateResponse>(
                    RateForQuery.GetRateForList, parameters, null);

                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
