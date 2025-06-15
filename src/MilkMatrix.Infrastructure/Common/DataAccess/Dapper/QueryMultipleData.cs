using System.Data;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.DataAccess.Dapper.Contracts;

namespace MilkMatrix.Infrastructure.Common.DataAccess.Dapper;

public class QueryMultipleData : IQueryMultipleData
{
    private ILogging logger;

    private readonly IRepositoryFactory repositoryFactory;

    public QueryMultipleData(
        IRepositoryFactory repositoryFactory,
        ILogging logger)
    {
        this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<object> GetMultiDetailsAsync(string query, string connKey, Dictionary<string, object>? parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure)
    {
        var repo = (IDapperRepository<object>)repositoryFactory.ConnectDapper<object>(connKey);
        if (repo == null)
        {
            throw new InvalidOperationException("Repository cannot be null");
        }

        try
        {
            return await repo.QueryMultipleAsync<object>(query, parameters, commandTimeOut, commandType);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetMultiDetailsAsync for query: {query}", ex);
            throw;
        }
    }

    public async Task<(IEnumerable<T1> t1, IEnumerable<T2> t2)> GetMultiDetailsAsync<T1, T2>(string query, string connKey, Dictionary<string, object>? parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure)
    {
        var repo = (IDapperRepository<object>)repositoryFactory.ConnectDapper<object>(connKey);
        if (repo == null)
        {
            throw new InvalidOperationException("Repository cannot be null");
        }

        try
        {
            using var gridReader = await repo.QueryMultipleAsync<object>(query, parameters, commandTimeOut, commandType);

            var result1 = (await gridReader.ReadAsync<T1>());
            var result2 = (await gridReader.ReadAsync<T2>());

            return (result1, result2);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetMultiDetailsAsync for query: {query}", ex);
            throw;
        }
    }

    public async Task<(IEnumerable<T1> t1, IEnumerable<T2> t2, IEnumerable<T3> t3)> GetMultiDetailsAsync<T1, T2, T3>(string query, string connKey, Dictionary<string, object>? parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure)
    {
        var repo = (IDapperRepository<object>)repositoryFactory.ConnectDapper<object>(connKey);
        if (repo == null)
        {
            throw new InvalidOperationException("Repository cannot be null");
        }

        try
        {
            using var gridReader = await repo.QueryMultipleAsync<object>(query, parameters, commandTimeOut, commandType);

            var result1 = (await gridReader.ReadAsync<T1>());
            var result2 = (await gridReader.ReadAsync<T2>());
            var result3 = (await gridReader.ReadAsync<T3>());

            return (result1, result2, result3);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetMultiDetailsAsync for query: {query}", ex);
            throw;
        }
    }

    public async Task<(IEnumerable<T1> t1, IEnumerable<T2> t2, IEnumerable<T3> t3, IEnumerable<T4> t4)> GetMultiDetailsAsync<T1, T2, T3, T4>(string query, string connKey, Dictionary<string, object>? parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure)
    {
        var repo = (IDapperRepository<object>)repositoryFactory.ConnectDapper<object>(connKey);
        if (repo == null)
        {
            throw new InvalidOperationException("Repository cannot be null");
        }

        try
        {
            using var gridReader = await repo.QueryMultipleAsync<object>(query, parameters, commandTimeOut, commandType);

            var result1 = (await gridReader.ReadAsync<T1>());
            var result2 = (await gridReader.ReadAsync<T2>());
            var result3 = (await gridReader.ReadAsync<T3>()); 
            var result4 = (await gridReader.ReadAsync<T4>());

            return (result1, result2, result3, result4);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetMultiDetailsAsync for query: {query}", ex);
            throw;
        }
    }

    public async Task<(IEnumerable<T1> t1, IEnumerable<T2> t2, IEnumerable<T3> t3, IEnumerable<T4> t4, IEnumerable<T5> t5)> GetMultiDetailsAsync<T1, T2, T3, T4, T5>(string query, string connKey, Dictionary<string, object>? parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure)
    {
        var repo = (IDapperRepository<object>)repositoryFactory.ConnectDapper<object>(connKey);
        if (repo == null)
        {
            throw new InvalidOperationException("Repository cannot be null");
        }

        try
        {
            using var gridReader = await repo.QueryMultipleAsync<object>(query, parameters, commandTimeOut, commandType);

            var result1 = (await gridReader.ReadAsync<T1>());
            var result2 = (await gridReader.ReadAsync<T2>());
            var result3 = (await gridReader.ReadAsync<T3>());
            var result4 = (await gridReader.ReadAsync<T4>());
            var result5 = (await gridReader.ReadAsync<T5>());

            return (result1, result2, result3, result4, result5);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetMultiDetailsAsync for query: {query}", ex);
            throw;
        }
    }
}
