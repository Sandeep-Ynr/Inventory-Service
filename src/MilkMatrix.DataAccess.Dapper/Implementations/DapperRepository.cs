using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MilkMatrix.DataAccess.Common.Repositories;
using MilkMatrix.DataAccess.Dapper.Contracts;

namespace MilkMatrix.DataAccess.Dapper.Implementations
{
    public class DapperRepository<T> : BaseRepository<T>, IDapperRepository<T> where T : class
    {
        public DapperRepository(string connectionString) : base(connectionString) { }

        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryAsync<T>("SELECT Id, Name FROM Users");
        }

        public override async Task<IEnumerable<T>> QueryAsync<T>(string query, Dictionary<string, object> parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);

                //Convert Dictionary to Dapper's DynamicParameters
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
                    param: parameters,
                    commandType: commandType,
                    commandTimeout: commandTimeOut
                );

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public async Task<SqlMapper.GridReader> QueryMultipleAsync<T>(string query, Dictionary<string, object>? parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
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
                throw;
            }
        }
    }
}
