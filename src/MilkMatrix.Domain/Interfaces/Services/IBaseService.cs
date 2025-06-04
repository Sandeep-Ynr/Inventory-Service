namespace MilkMatrix.Domain.Interfaces.Services;

public interface IBaseService<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T user);
    Task UpdateAsync(T user);
    Task DeleteAsync(int id);
}
