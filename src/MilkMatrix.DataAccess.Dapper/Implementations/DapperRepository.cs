using System.Data;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.DataAccess.Common.Repositories;
using MilkMatrix.DataAccess.Dapper.Contracts;

namespace MilkMatrix.DataAccess.Dapper.Implementations
{
    public class DapperRepository<T> : BaseRepository<T>, IDapperRepository<T> where T : class
    {
        public DapperRepository(string connectionString, ILogging logger) : base(connectionString, logger) { }

        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                using var conn = new SqlConnection(connString);
                return await conn.QueryAsync<T>("SELECT Id, Name FROM Users");
            }
            catch (Exception ex)
            {
                logger.LogError("Error in GetAllAsync", ex);
                throw;
            }
        }

        public override async Task<IEnumerable<T>> QueryAsync<T>(string query, Dictionary<string, object> parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using var conn = new SqlConnection(connString);
                var dynamicParams = new DynamicParameters();
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        dynamicParams.Add(param.Key, param.Value);
                    }
                }
                var result = await conn.QueryAsync<T>(
                    sql: query,
                    param: dynamicParams,
                    commandType: commandType,
                    commandTimeout: commandTimeOut
                );
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Error in QueryAsync", ex);
                throw;
            }
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync<T>(string query, Dictionary<string, object>? parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                var connection = new SqlConnection(connString);
                await connection.OpenAsync();

                var dynamicParams = new DynamicParameters();
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        if (param.Value == null ||
                            param.Value is string ||
                            param.Value.GetType().IsValueType)
                        {
                            dynamicParams.Add(param.Key, param.Value);
                        }
                    }
                }

                return await connection.QueryMultipleAsync(query, dynamicParams, commandTimeout: commandTimeOut ?? 30, commandType: commandType);
            }
            catch (Exception ex)
            {
                logger.LogError("Error in QueryMultipleAsync", ex);
                throw;
            }
        }

        public override async Task<T?> GetByIdAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using var conn = new SqlConnection(connString);
                var dynamicParams = new DynamicParameters();
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        if (param.Value == null ||
                            param.Value is string ||
                            param.Value.GetType().IsValueType)
                        {
                            dynamicParams.Add(param.Key, param.Value);
                        }
                    }
                }
                var result = await conn.QueryFirstOrDefaultAsync<T>(query, dynamicParams, commandType: commandType);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Error in GetByIdAsync", ex);
                throw;
            }
        }

        public override async Task<string> AddAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using var conn = new SqlConnection(connString);
                var dynamicParams = new DynamicParameters();
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        if (param.Value == null ||
                            param.Value is string ||
                            param.Value.GetType().IsValueType)
                        {
                            dynamicParams.Add(param.Key, param.Value);
                        }
                    }
                }
                dynamicParams.Add("@Message", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                await conn.ExecuteAsync(query, dynamicParams, commandType: commandType);
                return dynamicParams.Get<string>("@Message");
            }
            catch (Exception ex)
            {
                logger.LogError("Error in AddAsync", ex);
                throw;
            }
        }

        public override async Task<string> UpdateAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using var conn = new SqlConnection(connString);
                var dynamicParams = new DynamicParameters();
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        if (param.Value == null ||
                            param.Value is string ||
                            param.Value.GetType().IsValueType)
                        {
                            dynamicParams.Add(param.Key, param.Value);
                        }
                    }
                }
                dynamicParams.Add("@Message", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                await conn.ExecuteAsync(query, dynamicParams, commandType: commandType);
                return dynamicParams.Get<string>("@Message");
            }
            catch (Exception ex)
            {
                logger.LogError("Error in UpdateAsync", ex);
                throw;
            }
        }

        public override async Task<string> DeleteAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using var conn = new SqlConnection(connString);
                var dynamicParams = new DynamicParameters();
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        if (param.Value == null ||
                            param.Value is string ||
                            param.Value.GetType().IsValueType)
                        {
                            dynamicParams.Add(param.Key, param.Value);
                        }
                    }
                }
                dynamicParams.Add("@Message", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                await conn.ExecuteAsync(query, dynamicParams, commandType: commandType);
                return dynamicParams.Get<string>("@Message");
            }
            catch (Exception ex)
            {
                logger.LogError("Error in DeleteAsync", ex);
                throw;
            }
        }

        public override async Task<int> ExecuteScalarAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using var conn = new SqlConnection(connString);
                var dynamicParams = new DynamicParameters();
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        if (param.Value == null ||
                            param.Value is string ||
                            param.Value.GetType().IsValueType)
                        {
                            dynamicParams.Add(param.Key, param.Value);
                        }
                    }
                }
                var result = await conn.ExecuteScalarAsync<int>(query, dynamicParams, commandType: commandType);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("Error in ExecuteScalarAsync", ex);
                throw;
            }
        }

        public override async Task<int> BulkInsertAsync<T>(
            string tableName,
            IEnumerable<T> items,
            Dictionary<string, string> propertyToColumnMap,
            int batchSize = 1000)
        {
            using var connection = new SqlConnection(connString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                int totalInserted = 0;
                var props = typeof(T).GetProperties();
                var colList = propertyToColumnMap.Values.ToList();
                var itemList = items.ToList();

                for (int i = 0; i < itemList.Count; i += batchSize)
                {
                    var batch = itemList.Skip(i).Take(batchSize).ToList();
                    var sb = new StringBuilder();
                    var parameters = new DynamicParameters();

                    sb.Append($"INSERT INTO {tableName} ({string.Join(", ", colList)}) VALUES ");

                    var valueRows = new List<string>();
                    for (int colindex = 0; colindex < batch.Count; colindex++)
                    {
                        var valueParams = new List<string>();
                        foreach (var column in propertyToColumnMap)
                        {
                            var paramName = $"@{column.Value}_{colindex}";
                            valueParams.Add(paramName);

                            var prop = props.FirstOrDefault(p => p.Name.Equals(column.Key, StringComparison.OrdinalIgnoreCase));
                            parameters.Add(paramName, prop?.GetValue(batch[colindex], null));
                        }
                        valueRows.Add($"({string.Join(", ", valueParams)})");
                    }
                    sb.Append(string.Join(", ", valueRows));

                    totalInserted += await connection.ExecuteAsync(sb.ToString(), parameters, transaction);
                }

                transaction.Commit();
                return totalInserted;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
