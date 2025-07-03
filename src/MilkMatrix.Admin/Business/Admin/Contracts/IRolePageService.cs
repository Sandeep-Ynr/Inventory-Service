using MilkMatrix.Admin.Models.Admin.Requests.RolePage;
using MilkMatrix.Admin.Models.Admin.Responses.RolePage;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;

namespace MilkMatrix.Admin.Business.Admin.Contracts
{
    public interface IRolePageService
    {
        Task<IEnumerable<RolePages>> GetByIdAsync(int id);
        Task AddAsync(IEnumerable<RolePageInsertRequest> requests);
        Task UpdateAsync(RolePageUpdateRequest request);
        Task DeleteAsync(int id, int userId);
        Task<IListsResponse<RolePages>> GetAllAsync(IListsRequest request);
    }
}
