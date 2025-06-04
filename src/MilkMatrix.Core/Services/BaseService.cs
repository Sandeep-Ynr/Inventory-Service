using MilkMatrix.Domain.Interfaces.Repositories;
using MilkMatrix.Domain.Interfaces.Services;

namespace MilkMatrix.Core.Services;

public class BaseService<T> : IBaseService<T> where T : class
{
    protected readonly IBaseRepository<T> _repository;

    public BaseService(IBaseRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual Task<IEnumerable<T>> GetAllAsync() => _repository.GetAllAsync();

    public virtual Task<T?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

    public virtual Task AddAsync(T entity) => _repository.AddAsync(entity);

    public virtual Task UpdateAsync(T entity) => _repository.UpdateAsync(entity);

    public virtual Task DeleteAsync(int id) => _repository.DeleteAsync(id);
}
