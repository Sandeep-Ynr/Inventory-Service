using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Core.Extensions;

public static class FilterExtensions
{
    public static List<FilterCriteria> BuildFilterCriteriaFromRequest(
this IEnumerable<FiltersMeta> filterMetas,
Dictionary<string, object>? search)
    {
        var criteria = new List<FilterCriteria>();
        if (search == null) return criteria;

        foreach (var meta in filterMetas)
        {
            if (search.TryGetValue(meta.Key.ToLower(), out var value) && value != null)
            {
                // Restrict to allowed values if specified
                if (meta.ValuesAllowed != null && meta.ValuesAllowed.Any())
                {
                    if (!meta.ValuesAllowed.Contains(value.ToString()))
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
