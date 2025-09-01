using System.Data;
using Azure.Core;
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
using MilkMatrix.Milk.Contracts.Accounts.HSN;
using MilkMatrix.Milk.Contracts.Admin.GlobleSetting;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Accounts.HSN;
using MilkMatrix.Milk.Models.Response.Accounts.HSN;
using MilkMatrix.Milk.Models.Response.Bank;
using static MilkMatrix.Milk.Models.Queries.AccountsQueries;

namespace MilkMatrix.Milk.Implementations
{
    public class HSNService : IHSNService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public HSNService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(HSNService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task InsertHSN(HSNInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                    {
                        { "ActionType", (int)CrudActionType.Create },    // 1 = Insert, 2 = Update, 3 = Delete
                        { "BusinessId", request.BusinessId },
                        { "HSNCode", request.HSNCode },
                        { "Description", request.Description },
                        { "IGSTRate", request.IgstRate ?? (object)DBNull.Value },
                        { "CGSTRate", request.CgstRate ?? (object)DBNull.Value },
                        { "SGSTRate", request.SgstRate ?? (object)DBNull.Value },
                        { "CessRate", request.CessRate ?? (object)DBNull.Value },
                        { "CGSTInputLedgerId", request.CgstInputLedgerId ?? (object)DBNull.Value },
                        { "CGSTOutputLedgerId", request.CgstOutputLedgerId ?? (object)DBNull.Value },
                        { "SGSTInputLedgerId", request.SgstInputLedgerId ?? (object)DBNull.Value },
                        { "SGSTOutputLedgerId", request.SgstOutputLedgerId ?? (object)DBNull.Value },
                        { "IGSTInputLedgerId", request.IgstInputLedgerId ?? (object)DBNull.Value },
                        { "IGSTOutputLedgerId", request.IgstOutputLedgerId ?? (object)DBNull.Value },
                        { "CessInputLedgerId", request.CessInputLedgerId ?? (object)DBNull.Value },
                        { "CessOutputLedgerId", request.CessOutputLedgerId ?? (object)DBNull.Value },
                        { "WEFDate", request.WefDate ?? (object)DBNull.Value },
                        { "IsActive", request.IsActive },
                        { "CreatedBy", request.CreatedBy },
                       
                    };


                var message = await repository.AddAsync(AccountsQueries.InsupdHSN, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Account Ledger  {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Insert/Update Ledger: {request.HSNCode}", ex);
                throw;
            }

        }

        public async Task UpdateHSN(int id, HSNUpdateRequest request)

        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
          {
                        { "ActionType", (int)CrudActionType.Update },
                        { "id", id },
                        { "BusinessId", request.BusinessId },
                        { "HSNCode", request.HSNCode },
                        { "Description", request.Description },
                        { "IGSTRate", request.IgstRate ?? (object)DBNull.Value },
                        { "CGSTRate", request.CgstRate ?? (object)DBNull.Value },
                        { "SGSTRate", request.SgstRate ?? (object)DBNull.Value },
                        { "CessRate", request.CessRate ?? (object)DBNull.Value },
                        { "CGSTInputLedgerId", request.CgstInputLedgerId ?? (object)DBNull.Value },
                        { "CGSTOutputLedgerId", request.CgstOutputLedgerId ?? (object)DBNull.Value },
                        { "SGSTInputLedgerId", request.SgstInputLedgerId ?? (object)DBNull.Value },
                        { "SGSTOutputLedgerId", request.SgstOutputLedgerId ?? (object)DBNull.Value },
                        { "IGSTInputLedgerId", request.IgstInputLedgerId ?? (object)DBNull.Value },
                        { "IGSTOutputLedgerId", request.IgstOutputLedgerId ?? (object)DBNull.Value },
                        { "CessInputLedgerId", request.CessInputLedgerId ?? (object)DBNull.Value },
                        { "CessOutputLedgerId", request.CessOutputLedgerId ?? (object)DBNull.Value },
                        { "WEFDate", request.WefDate ?? (object)DBNull.Value },
                        { "IsActive", request.IsActive },
                        { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value }

          };

                var message = await repository.UpdateAsync(AccountsQueries.InsupdHSN, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Account Heads {request.HSNCode} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in Update HSN Code: {request.HSNCode}", ex);
                throw;
            }

        }






        public async Task<IListsResponse<HSNResponse>> GetHSNList(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<HSNResponse, int, FiltersMeta>(AccountsQueries.GetHSNList,
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
            return new ListsResponse<HSNResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<HSNResponse?> GetHSNById(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for HSN Code id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<HSNResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<HSNResponse>(AccountsQueries.GetHSNList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "Id", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new HSNResponse();
                logging.LogInfo(result != null
                    ? $" HSN Code with id {id} retrieved successfully."
                    : $" HSN Code with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for  HSN Code: {id}", ex);
                throw;
            }
        }


        public async Task DeleteHSNById(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType" , (int)CrudActionType.Delete },
                    {"id", id },
                    {"ModifyBy", userId }

                };

                var response = await repository.DeleteAsync(AccountsQueries.InsupdHSN, requestParams, CommandType.StoredProcedure);


                if (response.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {response}");
                }
                else
                {
                    logging.LogInfo($"HSN Code  deleted successfully.");
                }

                logging.LogInfo($"HSN Code  with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for HSN Code   id: {id}", ex);
                throw new Exception(ex.Message);
            }

        }




    }
}
