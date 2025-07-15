using System.Transactions;
using Azure.Core;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Requests.Approval.Level;
using MilkMatrix.Admin.Models.Admin.Responses.Approval.Details;
using MilkMatrix.Admin.Models.Admin.Responses.Approval.Level;
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
using MilkMatrix.Infrastructure.Common.Utils;
using static MilkMatrix.Admin.Models.Constants;
using InsertDetails = MilkMatrix.Admin.Models.Admin.Requests.Approval.Details.Insert;
using InsertLevel = MilkMatrix.Admin.Models.Admin.Requests.Approval.Level.Insert;

namespace MilkMatrix.Admin.Business.Admin.Implementation
{
    /// <summary>
    /// Approval Service Implementation
    /// </summary>
    public class ApprovalService : IApprovalService
    {
        private ILogging logger;

        private readonly IRepositoryFactory repositoryFactory;

        private readonly IQueryMultipleData queryMultipleData;

        /// <summary>
        /// Approval Service Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repositoryFactory"></param>
        /// <param name="queryMultipleData"></param>
        public ApprovalService(
            ILogging logger,
            IRepositoryFactory repositoryFactory,
            IQueryMultipleData queryMultipleData)
        {
            this.logger = logger.ForContext("ServiceName", nameof(ApprovalService));
            this.repositoryFactory = repositoryFactory;
            this.queryMultipleData = queryMultipleData;
        }

        ///<inheritdoc />
        public async Task AddAsync(IEnumerable<InsertLevel> requests)
        {
            if (requests == null || !requests.Any())
                throw new ArgumentNullException(nameof(requests), "Request cannot be null");

            requests.ForEach(async x =>
            {
                await DeleteAsync(x.PageId, x.UserId);
            });
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var request in requests)
                {
                    try
                    {

                        var repo = repositoryFactory.ConnectDapper<InsertLevel>(DbConstants.Main);

                        var parameters = new Dictionary<string, object>
                        {
                            ["BusinessId"] = request.BusinessId,
                            ["PageId"] = request.PageId,
                            ["UserId"] = request.UserId,
                            ["Sno"] = request.Level,
                            ["Status"] = true,
                            ["CreatedBy"] = request.CreatedBy,
                            ["ActionType"] = (int)CrudActionType.Create
                        };

                        await repo.AddAsync(ApprovalSpName.ApprovalUpsert, parameters);
                        logger.LogInfo($"approval {request.BusinessId} added successfully.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message, ex);
                        throw;
                    }
                }
                scope.Complete();
            }
        }

        ///<inheritdoc />
        public async Task AddDetailsAsync(IEnumerable<InsertDetails> requests)
        {
            if (requests == null || !requests.Any())
                throw new ArgumentNullException(nameof(requests), "Request cannot be null");

            requests.ForEach(async x =>
            {
                await DeleteAsync(x.PageId, x.UserId);
            });

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var request in requests)
                {
                    try
                    {
                        logger.LogInfo($"AddAsync called for approval details: {request.PageId}");

                        var repo = repositoryFactory.ConnectDapper<InsertDetails>(DbConstants.Main);

                        var parameters = new Dictionary<string, object>
                        {
                            ["BusinessId"] = request.BusinessId,
                            ["PageId"] = request.PageId,
                            ["UserId"] = request.UserId,
                            ["Sno"] = request.Level,
                            ["DocNumber"] = request.DocNumber,
                            ["SubCode"] = request.SubCode,
                            ["loginId"] = request.LoginId,
                            ["ActionType"] = (int)CrudActionType.Create
                        };

                        await repo.AddAsync(BusinessSpName.BusinessUpsert, parameters);
                        logger.LogInfo($"Approval {request.PageId} added successfully.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message, ex);
                        throw;
                    }
                }
                scope.Complete();
            }
        }

        ///<inheritdoc />
        public async Task DeleteAsync(int pageId, int userId)
        {
            try
            {
                logger.LogInfo($"DeleteAsync called for Configuration id: {pageId}");
                var repo = repositoryFactory.ConnectDapper<ApprovalResponse>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
            {
                {"Id", pageId },
                {"Status", false },
                {"ModifyBy", userId },
                {"ActionType" , (int)CrudActionType.Delete }
            };
                await repo.DeleteAsync(ApprovalSpName.ApprovalUpsert, parameters);
                logger.LogInfo($"Approval with id {pageId} deleted successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in DeleteAsync for approval id: {pageId}", ex);
                throw;
            }
        }

        ///<inheritdoc />
        public async Task<IListsResponse<ApprovalResponse>> GetAllAsync(IListsRequest request)
        {
            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<ApprovalResponse, int, FiltersMeta>(ApprovalSpName.GetApprovalLevels,
                DbConstants.Main,
                new Dictionary<string, object> { { "ActionType", (int)ReadActionType.All } },
                null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<ApprovalResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        ///<inheritdoc />
        public async Task<IEnumerable<ApprovalResponse>?> GetByIdAsync(int pageId)
        {
            try
            {
                logger.LogInfo($"GetByIdAsync called for Approval id: {pageId}");
                var repo = repositoryFactory
                           .ConnectDapper<ApprovalResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<ApprovalResponse>(ApprovalSpName.GetApprovalLevels, new Dictionary<string, object> { { "pageId", pageId },
                    { "ActionType", (int)ReadActionType.Individual } }, null);

                var result = data.Any() ? data : default;
                logger.LogInfo(result != null
                    ? $"Approval with id {pageId} retrieved successfully."
                    : $"Approval with id {pageId} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in GetByIdAsync for user id: {pageId}", ex);
                throw;
            }
        }

        ///<inheritdoc />
        public async Task UpdateAsync(Update request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request cannot be null");

            try
            {
                logger.LogInfo($"UpdateAsync called for update approval: {request.PageId}");

                var repo = repositoryFactory.ConnectDapper<Update>(DbConstants.Main);

                var parameters = new Dictionary<string, object>
                {
                    ["BusinessId"] = request.BusinessId,
                    ["PageId"] = request.PageId,
                    ["UserId"] = request.UserId,
                    ["Sno"] = request.Level,
                    ["DepartmentId"] = request.DepartmentId,
                    ["amount"] = request.Amount,
                    ["Status"] = request.IsActive,
                    ["ModifyBy"] = request.ModifyBy,
                    ["ActionType"] = (int)CrudActionType.Update
                };

                await repo.UpdateAsync(ApprovalSpName.ApprovalUpsert, parameters);
                logger.LogInfo($"Business {request.PageId} updated successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        ///<inheritdoc />
        public async Task<IListsResponse<ApprovalDetails>> GetAllDetailsAsync(IListsRequest request)
        {
            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<ApprovalDetails, int, FiltersMeta>(ApprovalSpName.GetApprovalLevels,
                DbConstants.Main,
                new Dictionary<string, object> { { "ActionType", (int)ReadActionType.All } },
                null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<ApprovalDetails>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
