using System.Linq.Expressions;
using System.Text.Json;
using MilkMatrix.Core.Entities.Filters;

namespace MilkMatrix.Core.Extensions;

public static class PagingExtensions
{
    public static IQueryable<T> ApplyFilters<T>(
        this IQueryable<T> query,
        IEnumerable<FilterCriteria>? filters,
        Action<string>? logWarning = null
    )
    {
        if (filters == null) return query;

        foreach (var filter in filters)
        {
            try
            {
                var param = Expression.Parameter(typeof(T), "x");
                var member = filter.Property.Split('.')
                    .Aggregate((Expression)param, Expression.PropertyOrField);

                var targetType = Nullable.GetUnderlyingType(member.Type) ?? member.Type;
                var convertedValue = ConvertFilterValue(filter.Value, targetType);

                var constant = Expression.Constant(convertedValue, member.Type);
                var body = filter.Operator.ToLowerInvariant() switch
                {
                    "eq" => Expression.Equal(member, constant),
                    "neq" => Expression.NotEqual(member, constant),
                    "gt" => Expression.GreaterThan(member, constant),
                    "gte" => Expression.GreaterThanOrEqual(member, constant),
                    "lt" => Expression.LessThan(member, constant),
                    "lte" => Expression.LessThanOrEqual(member, constant),
                    "contains" => BuildStringExpression(member, constant, "Contains"),
                    "startswith" => BuildStringExpression(member, constant, "StartsWith"),
                    "endswith" => BuildStringExpression(member, constant, "EndsWith"),
                    "between" => BuildBetweenExpression(member, filter.Value),
                    _ => LogAndReturnNull(logWarning, $"Unsupported operator '{filter.Operator}' for property '{filter.Property}'.")
                };

                if (body == null) continue;

                var lambda = Expression.Lambda<Func<T, bool>>(body, param);
                query = query.Where(lambda);
            }
            catch (Exception ex)
            {
                logWarning?.Invoke($"Error applying filter on property '{filter.Property}': {ex.Message}");
                continue;
            }
        }
        return query;
    }

    private static object? ConvertFilterValue(object? value, Type targetType)
    {
        if (value is JsonElement jsonElement)
        {
            return jsonElement.ValueKind switch
            {
                JsonValueKind.Number => targetType switch
                {
                    Type t when t == typeof(int) => jsonElement.GetInt32(),
                    Type t when t == typeof(double) => jsonElement.GetDouble(),
                    Type t when t == typeof(long) => jsonElement.GetInt64(),
                    _ => throw new InvalidOperationException($"Unsupported numeric type: {targetType}")
                },
                JsonValueKind.String => Convert.ChangeType(jsonElement.GetString(), targetType),
                JsonValueKind.True or JsonValueKind.False => Convert.ChangeType(jsonElement.GetBoolean(), targetType),
                _ => throw new InvalidOperationException($"Unsupported ValueKind: {jsonElement.ValueKind}")
            };
        }
        return value is not null ? Convert.ChangeType(value, targetType) : null;
    }

    private static Expression? BuildStringExpression(Expression member, Expression constant, string methodName)
    {
        var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;
        var memberToLower = Expression.Call(member, toLowerMethod);
        var constantToLower = Expression.Call(constant, toLowerMethod);
        var stringMethod = typeof(string).GetMethod(methodName, new[] { typeof(string) });
        return stringMethod != null
            ? Expression.Call(memberToLower, stringMethod, constantToLower)
            : null;
    }

    private static Expression? BuildBetweenExpression(Expression member, object? value)
    {
        if (value is string dateRange && !string.IsNullOrWhiteSpace(dateRange))
        {
            var dateParts = dateRange.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (dateParts.Length == 2 &&
                DateTime.TryParse(dateParts[0], out var startDate) &&
                DateTime.TryParse(dateParts[1], out var endDate))
            {
                var startConst = Expression.Constant(startDate, member.Type);
                var endConst = Expression.Constant(endDate, member.Type);
                var gte = Expression.GreaterThanOrEqual(member, startConst);
                var lte = Expression.LessThanOrEqual(member, endConst);
                return Expression.AndAlso(gte, lte);
            }
        }
        return null;
    }

    private static Expression? LogAndReturnNull(Action<string>? logWarning, string message)
    {
        logWarning?.Invoke(message);
        return null;
    }

    public static IQueryable<T> ApplySorting<T>(
    this IQueryable<T> query,
    IEnumerable<SortingCriteria>? sorts,
    Action<string>? logWarning = null // Optional logger for warnings
)
    {
        if (sorts == null || !sorts.Any()) return query;
        IOrderedQueryable<T>? orderedQuery = null;
        bool first = true;

        foreach (var sort in sorts)
        {
            try
            {
                var param = Expression.Parameter(typeof(T), "x");
                var member = sort.Property.Split('.').Aggregate((Expression)param, Expression.PropertyOrField);
                var lambda = Expression.Lambda(member, param);

                string? method = GetSortMethod(sort.Direction, first, logWarning);

                if (method == null)
                    continue;

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
            catch (Exception ex)
            {
                logWarning?.Invoke($"Error applying sort on property '{sort.Property}': {ex.Message}");
                continue;
            }
        }
        return orderedQuery ?? query;
    }

    private static string? GetSortMethod(string? direction, bool first, Action<string>? logWarning)
    {
        if (string.IsNullOrWhiteSpace(direction) || direction.Equals("ASC", StringComparison.OrdinalIgnoreCase))
            return first ? "OrderBy" : "ThenBy";
        if (direction.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            return first ? "OrderByDescending" : "ThenByDescending";

        logWarning?.Invoke($"Unsupported sort direction '{direction}'. Defaulting to ascending.");
        return first ? "OrderBy" : "ThenBy";
    }

    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingCriteria paging)
    {
        return paging.Limit > 0 ? query.Skip(paging.Offset).Take(paging.Limit) : query;
    }
}
