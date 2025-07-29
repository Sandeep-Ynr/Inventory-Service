using System.Net;
using System.Transactions;
using CsvHelper;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.Approval.Factory;
using MilkMatrix.Core.Abstractions.Approval.Service;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Request.Approval.Level;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Entities.Response.Approval.Details;
using MilkMatrix.Core.Entities.Response.Approval.Level;
using MilkMatrix.Core.Extensions;
using static MilkMatrix.Infrastructure.Common.Constants.Constants;
using InsertDetails = MilkMatrix.Core.Entities.Request.Approval.Details.Insert;
using InsertLevel = MilkMatrix.Core.Entities.Request.Approval.Level.Insert;

namespace MilkMatrix.Infrastructure.Common.Approval.Service
{
    /// <summary>
    /// Approval Service Implementation
    /// </summary>
    public class ApprovalService : IApprovalService
    {
        private ILogging logger;

        private readonly IRepositoryFactory repositoryFactory;

        private readonly IQueryMultipleData queryMultipleData;

        private readonly IApprovalFactory factory;

        private readonly AppConfig appConfig;

        /// <summary>
        /// Approval Service Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repositoryFactory"></param>
        /// <param name="queryMultipleData"></param>
        public ApprovalService(
            ILogging logger,
            IRepositoryFactory repositoryFactory,
            IQueryMultipleData queryMultipleData,
            IApprovalFactory factory,
            IOptions<AppConfig> appConfig)
        {
            this.logger = logger.ForContext("ServiceName", nameof(ApprovalService));
            this.repositoryFactory = repositoryFactory;
            this.queryMultipleData = queryMultipleData;
            this.factory = factory;
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "appConfig cannot be null"); ;
        }

        ///<inheritdoc />
        public async Task AddAsync(IEnumerable<InsertLevel> requests)
        {
            if (requests == null || !requests.Any())
                throw new ArgumentNullException(nameof(requests), "Request cannot be null");

            foreach (var x in requests)
            {
                if (x.PageId <= 0 || x.BusinessId <= 0)
                    throw new ArgumentException("PageId and BusinessId must be greater than zero.");
                logger.LogInfo($"AddAsync called for approval: {x.PageId} with BusinessId: {x.BusinessId}");
                await DeleteAsync(x.PageId, x.BusinessId);
            }

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
                            ["amount"] = request.Amount,
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

                    await repo.AddAsync(ApprovalSpName.ApprovalDetailsUpsert, parameters);
                    logger.LogInfo($"Approval {request.PageId} added successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                    throw;
                }
            }
        }

        ///<inheritdoc />
        public async Task DeleteAsync(int pageId, int businessId)
        {
            try
            {
                logger.LogInfo($"DeleteAsync called for Configuration id: {pageId}");
                var repo = repositoryFactory.ConnectDapper<ApprovalResponse>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
            {
                {"businessId", businessId },
                {"pageid", pageId },
                {"Status", false },
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
            return new ListsResponse<ApprovalResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        ///<inheritdoc />
        public async Task<IEnumerable<ApprovalResponse>?> GetByIdAsync(int pageId, int businessId)
        {
            try
            {
                logger.LogInfo($"GetByIdAsync called for Approval id: {pageId}");
                var repo = repositoryFactory
                           .ConnectDapper<ApprovalResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<ApprovalResponse>(ApprovalSpName.GetApprovalLevels, new Dictionary<string, object> { { "pageId", pageId },
                    {"businessId", businessId },
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
            return new ListsResponse<ApprovalDetails>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        ///<inheritdoc />
        public async Task<IEnumerable<ApprovalResponse>?> GetPageApprovalDetailsAsync(int pageId, int businessId, string recordId)
        {
            try
            {
                logger.LogInfo($"GetByIdAsync called for Approval id: {pageId}");
                var repo = repositoryFactory
                           .ConnectDapper<ApprovalResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<ApprovalResponse>(ApprovalSpName.GetPageApprovalDetails, new Dictionary<string, object> { { "pageId", pageId },
                    {"businessId", businessId },
                    {"RecordId", recordId },
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

        public async Task<StatusCode> ApproveAsync(IEnumerable<InsertDetails> requests, FactoryMapping handlerKey, Func<InsertDetails, Dictionary<string, object>> requestParams)
        {
            if (!requests.Any())
                throw new ArgumentNullException(nameof(requests), "Request cannot be null or empty");

            var handler = factory.GetHandler(handlerKey);
            var status = new StatusCode { Code = 0, Message = "No approvals processed." };

            if (handler == null)
            {
                status = new StatusCode { Code = (int)HttpStatusCode.InternalServerError, Message = "Approval handler not found." };
                return status;
            }

            foreach (var item in requests)
            {
                var parameters = requestParams(item);

                if (!await handler.CheckConditionsAsync(parameters))
                {
                    status = new StatusCode { Code = (int)HttpStatusCode.BadRequest, Message = "Approval conditions not met." };
                    continue;
                }

                requestParams(item).TryGetValue("UserId", out var userIdObj);
                if (item.UserId != Convert.ToInt32(userIdObj))
                {
                    status = new StatusCode { Code = (int)HttpStatusCode.Unauthorized, Message = "User not authorized to approve this request." };
                    continue;
                }

                await AddDetailsAsync(requests);

                if (appConfig.DefaultTopLevelApprover == item.Level)
                {
                    var approved = await handler.ApproveAsync(parameters);
                    if (approved)
                    {
                        status = new StatusCode { Code = (int)HttpStatusCode.OK, Message = "Approval successful." };
                    }
                    else
                    {
                        status = new StatusCode { Code = (int)HttpStatusCode.BadRequest, Message = "Approval failed." };
                    }
                }
                else
                {
                    status = new StatusCode { Code = (int)HttpStatusCode.OK, Message = "Approval added to queue for further processing." };
                }
            }

            return status;
        }
    }
}
