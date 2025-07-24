using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Responses.Report;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using static MilkMatrix.Admin.Models.Constants;

namespace MilkMatrix.Admin.Business.Admin.Implementation;

/// <summary>
/// Implements the reporting service for retrieving various logs and details in the system.
/// </summary>
public class ReportingService : IReportService
{
    private readonly IQueryMultipleData queryMultipleData;

    public ReportingService(IQueryMultipleData queryMultipleData)
    {
        this.queryMultipleData = queryMultipleData;
    }

    public async Task<IListsResponse<AuditLogs>> GetAuditLogsAsync(IListsRequest request)
    {
        var allowedFilterKeys = new[] { "UpdatedOn", "UserId" };
        var parameters = request.PrepareRequestParams(allowedFilterKeys, (int)ReadActionType.All);

        var (allResults, countResult, filterMetas) = await queryMultipleData
            .GetMultiDetailsAsync<AuditLogs, int, FiltersMeta>(
                ReportingSpName.GetAuditLogs, // Replace with your actual stored procedure name
                DbConstants.Main,
                parameters,
                null);

        var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search, allowedFilterKeys);
        var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
        var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

        var filtered = allResults.AsQueryable().ApplyFilters(filters);
        var sorted = filtered.ApplySorting(sorts);
        var paged = sorted.ApplyPaging(paging);

        var filteredCount = filtered.Count();

        return new ListsResponse<AuditLogs>
        {
            Count = filteredCount,
            Results = paged.ToList(),
            Filters = filterMetas
        };
    }

    public async Task<IListsResponse<EventLogs>> GetEventLogsAsync(IListsRequest request)
    {
        var allowedFilterKeys = new[] { "ActionDate", "UserId" };
        var parameters = request.PrepareRequestParams(allowedFilterKeys, (int)ReadActionType.All);

        var (allResults, countResult, filterMetas) = await queryMultipleData
            .GetMultiDetailsAsync<EventLogs, int, FiltersMeta>(
                ReportingSpName.GetEventLogs, // Replace with your actual stored procedure name
                DbConstants.Main,
                parameters,
                null);

        var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search, allowedFilterKeys);
        var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
        var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

        var filtered = allResults.AsQueryable().ApplyFilters(filters);
        var sorted = filtered.ApplySorting(sorts);
        var paged = sorted.ApplyPaging(paging);

        var filteredCount = filtered.Count();

        return new ListsResponse<EventLogs>
        {
            Count = filteredCount,
            Results = paged.ToList(),
            Filters = filterMetas
        };
    }

    public async Task<IListsResponse<LoginDetails>> GetLoginDetailsAsync(IListsRequest request)
    {
        var allowedFilterKeys = new[] { "LoginDate", "LogOutDate", "UserId" };
        var parameters = request.PrepareRequestParams(allowedFilterKeys, (int)ReadActionType.All);

        var (allResults, countResult, filterMetas) = await queryMultipleData
            .GetMultiDetailsAsync<LoginDetails, int, FiltersMeta>(
                ReportingSpName.GetLoginDetails, // Replace with your actual stored procedure name
                DbConstants.Main,
                parameters,
                null);

        var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search, allowedFilterKeys);
        var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
        var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

        var filtered = allResults.AsQueryable().ApplyFilters(filters);
        var sorted = filtered.ApplySorting(sorts);
        var paged = sorted.ApplyPaging(paging);

        var filteredCount = filtered.Count();

        return new ListsResponse<LoginDetails>
        {
            Count = filteredCount,
            Results = paged.ToList(),
            Filters = filterMetas
        };
    }
}
