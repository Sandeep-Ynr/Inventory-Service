namespace MilkMatrix.Core.Abstractions.Repository.Factories;

public interface IRepositoryFactory
{
    IBaseRepository<T> Connect<T>(string connKey) where T : class;
    IBaseRepository<T> ConnectDapper<T>(string connKey) where T : class;
}
