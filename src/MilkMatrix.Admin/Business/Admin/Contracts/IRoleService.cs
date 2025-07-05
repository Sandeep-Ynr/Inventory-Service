using MilkMatrix.Admin.Models.Admin.Requests.Role;
using MilkMatrix.Admin.Models.Admin.Responses.Role;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

public interface IRoleService
{
    Task<RoleDetails?> GetByIdAsync(int id);
    Task AddAsync(RoleInsertRequest request);
    Task UpdateAsync(RoleUpdateRequest request);
    Task DeleteAsync(int id, int userId);
    Task<IListsResponse<RoleDetails>> GetAllAsync(IListsRequest request);
}
