using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Common.Extensions;
using MilkMatrix.Admin.Models.Admin.Requests.Business;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Entities.Response.Business;
using MilkMatrix.Core.Extensions;
using static MilkMatrix.Admin.Models.Constants;

namespace MilkMatrix.Admin.Business.Admin.Implementation
{
    /// <summary>
    /// Provides business-related operations such as adding, updating, retrieving business details, and fetching business data by user ID.
    /// </summary>
    public class BusinessService : IBusinessService
    {
        private ILogging logger;

        private readonly IRepositoryFactory repositoryFactory;

        private readonly IQueryMultipleData queryMultipleData;

        private readonly AppConfig appConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessService"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repositoryFactory"></param>
        /// <param name="appConfig"></param>
        /// <param name="queryMultipleData"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BusinessService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig, IQueryMultipleData queryMultipleData)
        {
            this.logger = logger.ForContext("ServiceName", nameof(BusinessService));
            this.repositoryFactory = repositoryFactory;
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
            this.queryMultipleData = queryMultipleData ?? throw new ArgumentNullException(nameof(queryMultipleData), "QueryMultipleData cannot be null");
        }

        /// <inheritdoc/>
        public async Task AddAsync(BusinessInsertRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request cannot be null");

            try
            {
                logger.LogInfo($"AddAsync called for business: {request.Name}");

                var repo = repositoryFactory.ConnectDapper<BusinessInsertRequest>(DbConstants.Main);

                var parameters = new Dictionary<string, object>
                {
                    ["BusinessName"] = request.Name,
                    ["BusinessPrefix"] = request.Prefix,
                    ["BusinessAddress"] = request.Address,
                    ["ContactPerson"] = request.ContactPerson,
                    ["ContactMobile"] = request.ContactMobile,
                    ["Mobile"] = request.PhoneNo,
                    ["Email"] = request.EmailId,
                    ["Website"] = request.Website,
                    ["Esic"] = request.EsicNo,
                    ["GstNo"] = request.GstNo,
                    ["OpeningFinYear"] = request.OpFinancialYear,
                    ["OpeningFinDate"] = request.OpFromDate,
                    ["Pf"] = request.PfNo,
                    ["LockTransaction"] = request.LockData ?? false,
                    ["LockTransactionDate"] = request.LockBefore,
                    ["StateId"] = request.StateId,
                    ["BranchWiseSeq"] = request.BranchSequence,
                    ["Logo_Image_Id"] = request.LogoImageId,
                    ["BranchCode"] = request.BranchCode,
                    ["CreatedBy"] = request.CreatedBy,
                    ["IsStatus"] = true,
                    ["ActionType"] = (int)CrudActionType.Create
                };

                await repo.AddAsync(BusinessSpName.BusinessUpsert, parameters);
                logger.LogInfo($"Business {request.Name} added successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IListsResponse<BusinessDetails>> GetAllAsync(IListsRequest request)
        {
            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<BusinessDetails, int, FiltersMeta>(BusinessSpName.GetBusinessDetails,
                DbConstants.Main,
                new Dictionary<string, object> { { "ActionType", (int)ReadActionType.All }, { "Status", true } },
                null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.WithFullPath(appConfig.ApplicationDomain).AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<BusinessDetails>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        /// <inheritdoc/>
        public async Task<BusinessDetails?> GetByIdAsync(int id)
        {
            try
            {
                logger.LogInfo($"GetByIdAsync called for business id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<BusinessDetails>(DbConstants.Main);
                var data = await repo.QueryAsync<BusinessDetails>(BusinessSpName.GetBusinessDetails, new Dictionary<string, object> { { "BusinessId", id },
                                                                                { "ActionType", (int)ReadActionType.Individual } }, null);

                var result = data.Any() ? data.WithFullPath(appConfig.ApplicationDomain).FirstOrDefault() : default;
                logger.LogInfo(result != null
                    ? $"Business with id {id} retrieved successfully."
                    : $"Business with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in GetByIdAsync for user id: {id}", ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(BusinessUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request cannot be null");

            try
            {
                logger.LogInfo($"UpdateAsync called for business: {request.Name}");

                var repo = repositoryFactory.ConnectDapper<BusinessUpdateRequest>(DbConstants.Main);

                var parameters = new Dictionary<string, object>
                {
                    ["BusinessId"] = request.Id,
                    ["BusinessName"] = request.Name,
                    ["BusinessPrefix"] = request.Prefix,
                    ["BusinessAddress"] = request.Address,
                    ["ContactPerson"] = request.ContactPerson,
                    ["ContactMobile"] = request.ContactMobile,
                    ["Mobile"] = request.PhoneNo,
                    ["Email"] = request.EmailId,
                    ["Website"] = request.Website,
                    ["Esic"] = request.EsicNo,
                    ["GstNo"] = request.GstNo,
                    ["OpeningFinYear"] = request.OpFinancialYear,
                    ["OpeningFinDate"] = request.OpFromDate,
                    ["Pf"] = request.PfNo,
                    ["LockTransaction"] = request.LockData ?? false,
                    ["LockTransactionDate"] = request.LockBefore,
                    ["StateId"] = request.StateId,
                    ["BranchWiseSeq"] = request.BranchSequence,
                    ["Logo_Image_Id"] = request.LogoImageId,
                    ["BranchCode"] = request.BranchCode,
                    ["ModifyBy"] = request.ModifyBy,
                    ["IsStatus"] = request.IsActive,
                    ["ActionType"] = (int)CrudActionType.Update
                };

                await repo.UpdateAsync(BusinessSpName.BusinessUpsert, parameters);
                logger.LogInfo($"Business {request.Name} updated successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<BusinessMaster?>> GetBusinessDataByUserIdAsync(BusinessDataRequest request)
        {
            try
            {
                logger.LogInfo($"GetBusinessDataByUserIdAsync called for user id: {request.UserId}");
                var repo = repositoryFactory
                           .ConnectDapper<BusinessMaster>(DbConstants.Main);
                var data = await repo.QueryAsync<BusinessMaster>(BusinessSpName.GetBusinessData, new Dictionary<string, object> { { "UserId", request.UserId },
                                                                                { "ActionType", (int)request.ActionType } }, null);

                var result = data.Any() ? data : default;
                logger.LogInfo(result != null
                    ? $"Business with id {request.UserId} retrieved successfully."
                    : $"Business with id {request.UserId} not found.");
                return result ?? Enumerable.Empty<BusinessMaster>();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in GetByIdAsync for user id: {request.UserId}", ex);
                throw;
            }
        }
    }
}
