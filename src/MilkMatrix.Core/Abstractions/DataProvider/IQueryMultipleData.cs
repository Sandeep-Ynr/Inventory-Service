using System.Data;

namespace MilkMatrix.Core.Abstractions.DataProvider;

public interface IQueryMultipleData
{
    Task<(IEnumerable<T1> t1, IEnumerable<T2> t2)> GetMultiDetailsAsync<T1,T2>(string query, string connKey, Dictionary<string, object>? parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure);
    Task<(IEnumerable<T1> t1, IEnumerable<T2> t2, IEnumerable<T3> t3)> GetMultiDetailsAsync<T1, T2, T3>(string query, string connKey, Dictionary<string, object>? parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure);
    Task<(IEnumerable<T1> t1, IEnumerable<T2> t2, IEnumerable<T3> t3, IEnumerable<T4> t4)> GetMultiDetailsAsync<T1, T2, T3, T4>(string query, string connKey, Dictionary<string, object>? parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure);
    Task<(IEnumerable<T1> t1, IEnumerable<T2> t2, IEnumerable<T3> t3, IEnumerable<T4> t4, IEnumerable<T5> t5)> GetMultiDetailsAsync<T1, T2, T3, T4, T5>(string query, string connKey, Dictionary<string, object>? parameters, int? commandTimeOut, CommandType commandType = CommandType.StoredProcedure);
}
