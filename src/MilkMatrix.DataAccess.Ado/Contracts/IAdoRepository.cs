using MilkMatrix.Core.Abstractions.Repository;

namespace MilkMatrix.DataAccess.Ado.Contracts;

public interface IAdoRepository<T> : IBaseRepository<T> where T : class
{
}
