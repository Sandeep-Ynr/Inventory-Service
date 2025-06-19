using MilkMatrix.Admin.Models.Admin.Requests.RolePage;
using MilkMatrix.Admin.Models.Admin.Responses.Role;
using MilkMatrix.Admin.Models.Admin.Responses.RolePage;

namespace MilkMatrix.Admin.Business.Admin.Contracts
{
    public interface IRolePageService
    {
        Task<IEnumerable<RolePages>> GetByIdAsync(int id, int businessId);
        Task AddAsync(RolePageInsertRequest request);
        Task UpdateAsync(RolePageUpdateRequest request);
        Task DeleteAsync(int id, int businessId, int userId);
    }
}
