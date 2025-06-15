using System.Data;
using Dapper;
using MilkMatrix.Core.Abstractions.Repository;

namespace MilkMatrix.DataAccess.Dapper.Contracts;

public interface IDapperRepository<T> : IBaseRepository<T> where T : class
{
    /// <summary>
    /// Use this method when we have multiple result sets of records
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="parameters"></param>
    /// <param name="commandTimeOut"></param>
    /// <param name="commandType"></param>
    /// <returns>It executes multiple queries asynchronously within the same command and maps the results to strong entities.</returns>
    Task<SqlMapper.GridReader> QueryMultipleAsync<T>(string query, Dictionary<string, object> parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure);
}
