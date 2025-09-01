using MilkMatrix.Admin.Models.Admin.Requests.User;
using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

/// <summary>
/// Interface for user management services.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves user details by user ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserDetails?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new user or updates an existing user based on the request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<string> AddAsync(UserInsertRequest request);

    /// <summary>
    /// Updates an existing user based on the request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<string> UpdateAsync(UserUpdateRequest request);

    /// <summary>
    /// Updates the user profile based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<string> UpdateProfileAsync(UserProfileUpdate request);

    /// <summary>
    /// Deletes a user by ID and user ID of the person performing the deletion.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteAsync(int id, int userId);

    /// <summary>
    /// Retrieves a list of users based on the provided request and user ID.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IListsResponse<Users>> GetAllAsync(IListsRequest request, int userId);

    /// <summary>
    /// Adds users based on the request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task AddBulkUsersAsync(byte[] bytes, int userId);

    /// <summary>
    /// Retrieves user notifications by user ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IEnumerable<UserApprovalNotification>> GetNotificationsByIdAsync(int id);
}
