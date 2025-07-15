using System.Transactions;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Requests.Rejection;
using MilkMatrix.Admin.Models.Admin.Responses.Rejection;
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
using static MilkMatrix.Admin.Models.Constants;

namespace MilkMatrix.Admin.Business.Admin.Implementation;

public class RejectionService : IRejectionService
{

    private ILogging logger;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly IQueryMultipleData queryMultipleData;

    /// <summary>
    /// Rejection Service Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="repositoryFactory"></param>
    /// <param name="queryMultipleData"></param>
    public RejectionService(
        ILogging logger,
        IRepositoryFactory repositoryFactory,
        IQueryMultipleData queryMultipleData)
    {
        this.logger = logger.ForContext("ServiceName", nameof(RejectionService));
        this.repositoryFactory = repositoryFactory;
        this.queryMultipleData = queryMultipleData;
    }
    public async Task AddAsync(IEnumerable<InsertRejection> requests)
    {
        if (requests == null || !requests.Any())
            throw new ArgumentNullException(nameof(requests), "Request cannot be null");
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            foreach (var request in requests)
            {
                try
                {
                    logger.LogInfo($"AddAsync called for rejection: {request.PageId}");

                    var repo = repositoryFactory.ConnectDapper<InsertRejection>(DbConstants.Main);

                    var parameters = new Dictionary<string, object>
                    {
                        ["BusinessId"] = request.BusinessId,
                        ["PageId"] = request.PageId,
                        ["UserId"] = request.UserId,
                        ["Sno"] = request.Level,
                        ["DocNo"] = request.DocNo,
                        ["Reason"] = request.Reason,
                        ["RejectedBy"] = request.RejectedBy,
                        ["ActionType"] = (int)CrudActionType.Create
                    };

                    await repo.AddAsync(RejectionSpName.RejectionInsert, parameters);
                    logger.LogInfo($"Rejection {request.BusinessId} added successfully.");
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

    public async Task<IListsResponse<Details>> GetAllAsync(IListsRequest request)
    {
        // 1. Fetch all results, count, and filter meta from stored procedure
        var (allResults, countResult, filterMetas) = await queryMultipleData
            .GetMultiDetailsAsync<Details, int, FiltersMeta>(RejectionSpName.GetRejectioDetails,
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
        return new ListsResponse<Details>
        {
            Count = filteredCount,
            Results = paged.ToList(),
            Filters = filterMetas
        };
    }
}
