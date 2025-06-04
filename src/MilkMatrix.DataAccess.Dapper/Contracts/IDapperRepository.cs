using MilkMatrix.Domain.Interfaces.Repositories;

namespace MilkMatrix.DataAccess.Dapper.Contracts;

public interface IDapperRepository<T> : IBaseRepository<T> where T : class
{
}
