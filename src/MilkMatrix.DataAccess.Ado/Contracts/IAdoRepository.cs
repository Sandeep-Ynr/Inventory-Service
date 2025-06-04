using MilkMatrix.Domain.Interfaces.Repositories;

namespace MilkMatrix.DataAccess.Ado.Contracts;

public interface IAdoRepository<T> : IBaseRepository<T> where T : class
{
}
