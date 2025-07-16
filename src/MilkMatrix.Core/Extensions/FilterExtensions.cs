using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Core.Extensions;

public static class FilterExtensions
{
    public static List<FilterCriteria> BuildFilterCriteriaFromRequest(
     this IEnumerable<FiltersMeta> filterMetas,
     Dictionary<string, object>? filters, string? search)
    {
        var criteria = new List<FilterCriteria>();

        if (!string.IsNullOrWhiteSpace(search))
        {
            // Build a FilterCriteria for global search
            var globalFilter = new FilterCriteria
            {
                Property = Constants.SearchString,
                Operator = "contains",
                Value = search
            };

            // Insert globalFilter at the start of filters
            criteria = criteria?.ToList() ?? new List<FilterCriteria>();
            criteria.Insert(0, globalFilter);
        }

        if (filters == null) return criteria;

        // Create a case-insensitive dictionary for search keys
        var searchCI = new Dictionary<string, object>(filters, StringComparer.OrdinalIgnoreCase);

        foreach (var meta in filterMetas)
        {
            // Try to get the value using the original key (case-insensitive)
            if (searchCI.TryGetValue(meta.Key, out var value) && value != null)
            {
                // Restrict to allowed values if specified
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
}
