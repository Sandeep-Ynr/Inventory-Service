using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Core.Extensions;

public static class FilterExtensions
{
    public static List<FilterCriteria> BuildFilterCriteriaFromRequest(
    this IEnumerable<FiltersMeta> filterMetas,
    Dictionary<string, object>? filters,
    string? search,
    IEnumerable<string>? ignoreKeys = null)
    {
        var criteria = new List<FilterCriteria>();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var globalFilter = new FilterCriteria
            {
                Property = Constants.SearchString,
                Operator = "contains",
                Value = search
            };
            criteria.Insert(0, globalFilter);
        }

        if (filters == null) return criteria;

        var searchCI = new Dictionary<string, object>(filters, StringComparer.OrdinalIgnoreCase);
        var ignoreSet = ignoreKeys != null
            ? new HashSet<string>(ignoreKeys, StringComparer.OrdinalIgnoreCase)
            : new HashSet<string>();

        foreach (var meta in filterMetas)
        {
            // Skip ignored keys
            if (ignoreSet.Contains(meta.Key))
                continue;

            if (searchCI.TryGetValue(meta.Key, out var value) && value != null)
            {
                if (meta.ValuesAllowed != null && meta.ValuesAllowed.Any())
                {
                    if (!meta.ValuesAllowed.Contains(value.ToString(), StringComparer.OrdinalIgnoreCase))
                        continue;
                }

                var op = meta.Operator ?? (meta.Type == "string" ? "contains" : "eq");
                criteria.Add(new FilterCriteria
                {
                    Property = meta.Key,
                    Operator = op,
                    Value = value
                });
            }
        }
        return criteria;
    }

    public static List<SortingCriteria> BuildSortCriteriaFromRequest(
        this IEnumerable<FiltersMeta> filterMetas,
        Dictionary<string, object>? sort)
    {
        var criteria = new List<SortingCriteria>();
        if (sort == null) return criteria;

        foreach (var meta in filterMetas)
        {
            if (sort.TryGetValue(meta.Key, out var dirObj) && dirObj != null)
            {
                var dir = dirObj.ToString()?.ToUpperInvariant() == "DESC" ? "DESC" : "ASC";
                criteria.Add(new SortingCriteria
                {
                    Property = meta.Key,
                    Direction = dir
                });
            }
        }
        return criteria;
    }

    /// <summary>
    /// Builds a parameter dictionary from the Filters dictionary of an IListsRequest,
    /// including only allowed keys (case-insensitive).
    /// </summary>
    public static Dictionary<string, object> PrepareRequestParams(
        this IListsRequest request,
        IEnumerable<string> allowedKeys,
       int actionType = 1)
    {
        var parameters = new Dictionary<string, object>
    {
        { "ActionType", actionType }
    };

        if (request.Filters == null)
            return parameters;

        var allowed = new HashSet<string>(allowedKeys, StringComparer.OrdinalIgnoreCase);

        foreach (var kvp in request.Filters)
        {
            if (kvp.Value != null && allowed.Contains(kvp.Key))
                parameters[kvp.Key] = kvp.Value;
        }

        return parameters;
    }
}
