using MilkMatrix.Domain.Entities.Dtos;
using MilkMatrix.Domain.Interfaces.Services;
using MilkMatrix.Infrastructure.Contracts.Repositories;

namespace MilkMatrix.Core.Services;

public class UserService : BaseService<User>, IUserService
{
    private readonly IRepositoryFactory _repoFactory;

    public UserService(IRepositoryFactory repoFactory)
        : base(repoFactory.Connect<User>("DbConstants.Main"))
    {
        _repoFactory = repoFactory;
    }

    public async Task<IEnumerable<User>> GetAllWithAdoAsync(string connKey)
        => await _repoFactory.Connect<User>(connKey).GetAllAsync();

    public async Task<IEnumerable<User>> GetAllWithDapperAsync(string connKey)
        => await _repoFactory.ConnectDapper<User>(connKey).GetAllAsync();
}
