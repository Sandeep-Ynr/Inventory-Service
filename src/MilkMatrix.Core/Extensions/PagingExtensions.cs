using System.Linq.Expressions;
using System.Text.Json;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;

namespace MilkMatrix.Core.Extensions;

public static class PagingExtensions
{
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IEnumerable<FilterCriteria>? filters)
    {
        if (filters == null) return query;
        foreach (var filter in filters)
        {
            Expression? body = null;
            var param = Expression.Parameter(typeof(T), "x");
            var member = filter.Property.Split('.')
                .Aggregate((Expression)param, Expression.PropertyOrField);

            var targetType = Nullable.GetUnderlyingType(member.Type) ?? member.Type;

            object? convertedValue = null;

            if (filter.Value is JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == JsonValueKind.Number)
                {
                    // Example: try parsing as double or int depending on your needs
                    if (targetType == typeof(int))
                        convertedValue = jsonElement.GetInt32();
                    else if (targetType == typeof(double))
                        convertedValue = jsonElement.GetDouble();
                    else if (targetType == typeof(long))
                        convertedValue = jsonElement.GetInt64();
                    else
                        throw new InvalidOperationException($"Unsupported numeric type: {targetType}");
                }
                else if (jsonElement.ValueKind == JsonValueKind.String)
                {
                    var strVal = jsonElement.GetString()?.ToLower();
                    convertedValue = Convert.ChangeType(strVal, targetType);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported ValueKind: {jsonElement.ValueKind}");
                }
            }
            else
            {
                convertedValue = Convert.ChangeType(filter.Value, targetType);
            }

            var constant = Expression.Constant(convertedValue, member.Type);
            switch (filter.Operator.ToLowerInvariant())
            {
                case "eq": body = Expression.Equal(member, constant); break;
                case "neq": body = Expression.NotEqual(member, constant); break;
                case "gt": body = Expression.GreaterThan(member, constant); break;
                case "gte": body = Expression.GreaterThanOrEqual(member, constant); break;
                case "lt": body = Expression.LessThan(member, constant); break;
                case "lte": body = Expression.LessThanOrEqual(member, constant); break;
                case "contains":
                    body = Expression.Call(member, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, constant); break;
                case "startswith":
                    body = Expression.Call(member, typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!, constant); break;
                case "endswith":
                    body = Expression.Call(member, typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!, constant); break;
                case "between":
                    if (filter.Value is string dateRange && !string.IsNullOrWhiteSpace(dateRange))
                    {
                        // Support "date1-date2" or "date1 - date2"
                        var dateParts = dateRange.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        if (dateParts.Length == 2 &&
                            DateTime.TryParse(dateParts[0], out var startDate) &&
                            DateTime.TryParse(dateParts[1], out var endDate))
                        {
                            var startConst = Expression.Constant(startDate, member.Type);
                            var endConst = Expression.Constant(endDate, member.Type);
                            var gte = Expression.GreaterThanOrEqual(member, startConst);
                            var lte = Expression.LessThanOrEqual(member, endConst);
                            body = Expression.AndAlso(gte, lte);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                    break;
                default: continue;
            }

            var lambda = Expression.Lambda<Func<T, bool>>(body!, param);
            query = query.Where(lambda);
        }
        return query;
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, IEnumerable<SortingCriteria>? sorts)
    {
        if (sorts == null || !sorts.Any()) return query;
        IOrderedQueryable<T>? orderedQuery = null;
        bool first = true;
        foreach (var sort in sorts)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var member = sort.Property.Split('.').Aggregate((Expression)param, Expression.PropertyOrField);
            var lambda = Expression.Lambda(member, param);

            string method = (sort.Direction?.ToUpperInvariant() == "DESC")
                ? (first ? "OrderByDescending" : "ThenByDescending")
                : (first ? "OrderBy" : "ThenBy");

            orderedQuery = (orderedQuery == null)
                ? (IOrderedQueryable<T>)typeof(Queryable).GetMethods()
                    .First(m => m.Name == method && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), member.Type)
                    .Invoke(null, new object[] { query, lambda })!
                : (IOrderedQueryable<T>)typeof(Queryable).GetMethods()
                    .First(m => m.Name == method && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), member.Type)
                    .Invoke(null, new object[] { orderedQuery, lambda })!;
            first = false;
        }
        return orderedQuery ?? query;
    }

    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingCriteria paging)
    {
        return query.Skip(paging.Offset).Take(paging.Limit);
    }
}
