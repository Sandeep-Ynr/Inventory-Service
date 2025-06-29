using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings;
using MilkMatrix.Admin.Models.Admin.Responses.ConfigurationSettings;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

/// <summary>
/// Defines the contract for managing application configuration settings like email/sms/tagconfigs etc..
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Retrieves the details of a configuration setting or tag by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ConfigurationDetails?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new configuration setting or tag to the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task AddAsync(ConfigurationInsertRequest request);

    /// <summary>
    /// Updates an existing configuration setting or tag in the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task UpdateAsync(ConfigurationUpdateRequest request);

    /// <summary>
    /// Deletes a configuration setting or tag from the system based on its unique identifier and the user who requested the deletion.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteAsync(int id, int userId);

    /// <summary>
    /// Retrieves a list of configuration settings or tags from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IListsResponse<ConfigurationDetails>> GetAllAsync(IListsRequest request, int userId);
}
