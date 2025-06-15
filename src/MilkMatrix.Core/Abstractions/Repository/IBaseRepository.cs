using System.Data;

namespace MilkMatrix.Core.Abstractions.Repository;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure);
    Task<string> AddAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure);
    Task<string> UpdateAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure);
    Task<string> DeleteAsync(string query, Dictionary<string, object> parameters, CommandType commandType = CommandType.StoredProcedure);

    /// <summary>
    /// Use this method when we need set of records or need to get data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="parameters"></param>
    /// <param name="commandTimeOut"></param>
    /// <param name="commandType"></param>
    /// <returns>It executes a query asynchronously and returns the results of the query as an IEnumerable<T>.</returns>
    Task<IEnumerable<T>> QueryAsync<T>(string query, Dictionary<string, object> parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure);

}
