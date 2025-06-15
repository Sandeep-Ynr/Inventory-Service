using System.Collections.Concurrent;
using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository;
using MilkMatrix.Domain.Entities.Dtos;

namespace MilkMatrix.DataAccess.Common.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly string _connectionString;

    protected readonly ILogging logger;
    // PropertyInfo cache for fast property lookup
    private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> cachedProperty = new();

    public BaseRepository(string connectionString, ILogging logger)
    {
        _connectionString = connectionString;
        this.logger = logger.ForContext("Repository", nameof(BaseRepository<T>));
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError("Error in GetAllAsync", ex);
            throw;
        }
    }

    public virtual async Task<T?> GetByIdAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn)
            {
                CommandType = commandType
            };
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var obj = Activator.CreateInstance<T>();
                var props = GetPropertiesForType(typeof(T));
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var colName = reader.GetName(i);
                    if (props.TryGetValue(colName, out var prop) && !await reader.IsDBNullAsync(i))
                    {
                        prop.SetValue(obj, reader.GetValue(i));
                    }
                }
                return obj;
            }
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError("Error in GetByIdAsync", ex);
            throw;
        }
    }

    public virtual async Task<string> AddAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn)
            {
                CommandType = commandType
            };
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }
            var outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return outputParam.Value?.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError("Error in AddAsync", ex);
            throw;
        }
    }

    public virtual async Task<string> UpdateAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn)
            {
                CommandType = commandType
            };
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }
            var outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return outputParam.Value?.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError("Error in UpdateAsync", ex);
            throw;
        }
    }

    public virtual async Task<string> DeleteAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn)
            {
                CommandType = commandType
            };
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }
            var outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 200)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return outputParam.Value?.ToString();
        }
        catch (Exception ex)
        {
            logger.LogError("Error in DeleteAsync", ex);
            throw;
        }
    }

    public virtual async Task<IEnumerable<T>> QueryAsync<T>(string query, Dictionary<string, object> parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            var result = new List<T>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn)
            {
                CommandType = commandType,
                CommandTimeout = commandTimeOut ?? 30
            };

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
        catch (Exception ex)
        {
            logger.LogError("Error in QueryAsync", ex);
            throw;
        }
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
