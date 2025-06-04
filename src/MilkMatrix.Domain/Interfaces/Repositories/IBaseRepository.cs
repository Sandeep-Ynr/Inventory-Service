using System.Data;

namespace MilkMatrix.Domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);

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
