using MilkMatrix.Admin.Models.Admin.Requests.User;
using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

public interface IUserService
{
    Task<UserDetails?> GetByIdAsync(int id);
    Task AddAsync(UserInsertRequest request);
    Task UpdateAsync(UserUpdateRequest request);
    Task DeleteAsync(int id, int userId);

    Task<IListsResponse<UserDetails>> GetAllAsync(IListsRequest request);
}
