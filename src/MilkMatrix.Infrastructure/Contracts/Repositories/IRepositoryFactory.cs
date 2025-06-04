using MilkMatrix.DataAccess.Ado.Contracts;
using MilkMatrix.DataAccess.Dapper.Contracts;

namespace MilkMatrix.Infrastructure.Contracts.Repositories;

public interface IRepositoryFactory
{
    IAdoRepository<T> Connect<T>(string connKey) where T : class;
    IDapperRepository<T> ConnectDapper<T>(string connKey) where T : class;
}
