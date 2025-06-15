using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.DataAccess.Ado.Contracts;
using MilkMatrix.DataAccess.Common.Repositories;

namespace MilkMatrix.DataAccess.Ado.Implementations;

public class AdoRepository<T> : BaseRepository<T>, IAdoRepository<T> where T : class
{
    public AdoRepository(string connectionString, ILogging logger) : base(connectionString, logger) { }
}
