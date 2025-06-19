using MilkMatrix.Admin.Models.Admin.Requests.User;
using MilkMatrix.Admin.Models.Admin.Responses.User;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

public interface IUserService
{
    Task<UserDetails?> GetByIdAsync(int id);
    Task AddAsync(UserInsertRequest request);
    Task UpdateAsync(UserUpdateRequest request);
    Task DeleteAsync(int id, int userId);

    Task<IEnumerable<UserDetails>> GetAllAsync(Dictionary<string, object> filters);
}
