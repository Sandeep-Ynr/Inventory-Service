using MilkMatrix.Domain.Entities.Dtos;
using MilkMatrix.Domain.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Collections.Concurrent;

namespace MilkMatrix.DataAccess.Common.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly string _connectionString;

    // PropertyInfo cache for fast property lookup
    private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> cachedProperty = new();

    public BaseRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        var result = new List<T>();
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("SELECT Id, Name FROM Users", conn);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new User { Id = reader.GetInt32(0), Name = reader.GetString(1) } as T);
        }
        return result;
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        // Default: throw, or implement for common entities
        throw new NotImplementedException("Override in derived class or specialize for T.");
    }

    public virtual async Task AddAsync(T entity)
    {
        // Default: throw, or implement for common entities
        throw new NotImplementedException("Override in derived class or specialize for T.");
    }

    public virtual async Task UpdateAsync(T entity)
    {
        // Default: throw, or implement for common entities
        throw new NotImplementedException("Override in derived class or specialize for T.");
    }

    public virtual async Task DeleteAsync(int id)
    {
        // Default: throw, or implement for common entities
        throw new NotImplementedException("Override in derived class or specialize for T.");
    }

    public virtual async Task<IEnumerable<T>> QueryAsync<T>(string query, Dictionary<string, object> parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure)
    {
        var result = new List<T>();
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(query, conn)
        {
            CommandType = commandType,
            CommandTimeout = commandTimeOut ?? 30
        };

        // Add parameters
        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
            }
        }

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        var isPrimitive = typeof(T).IsPrimitive || typeof(T) == typeof(string) || typeof(T) == typeof(decimal);

        // Use cached property info for POCO mapping
        var props = isPrimitive ? null : GetPropertiesForType(typeof(T));

        while (await reader.ReadAsync())
        {
            if (isPrimitive)
            {
                result.Add((T)Convert.ChangeType(reader[0], typeof(T)));
            }
            else
            {
                var obj = Activator.CreateInstance<T>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var colName = reader.GetName(i);
                    if (props != null && props.TryGetValue(colName, out var prop) && !await reader.IsDBNullAsync(i))
                    {
                        prop.SetValue(obj, reader.GetValue(i));
                    }
                }
                result.Add(obj);
            }
        }

        return result;
    }

    // Caches property info for each type for fast lookup
    private static Dictionary<string, PropertyInfo> GetPropertiesForType(Type type)
    {
        if (!cachedProperty.TryGetValue(type, out var props))
        {
            props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);
            cachedProperty[type] = props;
        }
        return props;
    }
}
