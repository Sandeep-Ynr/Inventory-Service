using Microsoft.Extensions.Configuration;
using MilkMatrix.DataAccess.Ado.Contracts;
using MilkMatrix.DataAccess.Ado.Implementations;
using MilkMatrix.DataAccess.Dapper.Contracts;
using MilkMatrix.DataAccess.Dapper.Implementations;
using MilkMatrix.Infrastructure.Contracts.Repositories;

namespace MilkMatrix.Infrastructure.Factories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly IConfiguration _configuration;
    public RepositoryFactory(IConfiguration configuration) => _configuration = configuration;

    public IAdoRepository<T> Connect<T>(string connKey) where T : class =>
        (IAdoRepository<T>)Activator.CreateInstance(typeof(AdoRepository<>).MakeGenericType(typeof(T)), _configuration.GetConnectionString(connKey))!;

    public IDapperRepository<T> ConnectDapper<T>(string connKey) where T : class =>
        (IDapperRepository<T>)Activator.CreateInstance(typeof(DapperRepository<>).MakeGenericType(typeof(T)), _configuration.GetConnectionString(connKey))!;
}
