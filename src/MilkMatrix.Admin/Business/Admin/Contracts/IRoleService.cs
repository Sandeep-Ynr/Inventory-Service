using MilkMatrix.Admin.Models.Admin.Requests.Role;
using MilkMatrix.Admin.Models.Admin.Responses.Role;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

public interface IRoleService
{
    Task<Roles?> GetByIdAsync(int id);
    Task AddAsync(RoleInsertRequest request);
    Task UpdateAsync(RoleUpdateRequest request);
    Task DeleteAsync(int id, int userId);

    Task<IEnumerable<Roles>> GetAllAsync(Dictionary<string, object> filters);
}
