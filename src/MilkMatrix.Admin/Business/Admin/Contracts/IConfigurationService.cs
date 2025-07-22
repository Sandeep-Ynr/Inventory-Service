using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.BlockedMobiles;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.CommonStatus;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.Configurations;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.Email;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings.Sms;
using MilkMatrix.Admin.Models.Admin.Responses.ConfigurationSettings;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;

namespace MilkMatrix.Admin.Business.Admin.Contracts;

/// <summary>
/// Defines the contract for managing application configuration settings like email/sms/tagconfigs etc..
/// </summary>
public interface IConfigurationService
{
    #region Configuration Settings
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
    #endregion

    #region Email Settings
    /// <summary>
    /// Retrieves the details of a smtp setting or tag by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SmtpDetails?> GetBySmtpIdAsync(int id);

    /// <summary>
    /// Adds a new smtp setting system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task AddSmtpDetailsAsync(SmtpSettingsInsert request);

    /// <summary>
    /// Updates an existing smtp setting in the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task UpdateSmtpDetailsAsync(SmtpSettingsUpdate request);

    /// <summary>
    /// Deletes a smtp setting from the system based on its unique identifier and the user who requested the deletion.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteSmtpDetailsAsync(int id, int userId);

    /// <summary>
    /// Retrieves a list of smtp settings from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IListsResponse<SmtpDetails>> GetAllSmtpDetaisAsync(IListsRequest request);
    #endregion

    #region Mobile blocked Settings
    /// <summary>
    /// Retrieves the details of a smtp setting or tag by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BlockedMobiles?> GetBlockedMobilesAsync(int id);

    /// <summary>
    /// Adds a new smtp setting system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task AddMobileBlockAsync(BlockedMobilesInsert request);

    /// <summary>
    /// Updates an existing smtp setting in the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task UpdateMobileBlockAsync(BlockedMobilesUpdate request);

    /// <summary>
    /// Deletes a smtp setting from the system based on its unique identifier and the user who requested the deletion.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteMobileBlockAsync(int id, int userId);

    /// <summary>
    /// Retrieves a list of smtp settings from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IListsResponse<BlockedMobiles>> GetAllBlockedMobilesAsync(IListsRequest request);
    #endregion

    #region Sms Settings
    /// <summary>
    /// Retrieves the details of a sms setting by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SmsControlDetails?> GetBySmsControlByIdAsync(int id);

    /// <summary>
    /// Adds a new sms setting system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task AddSmsControlDetailsAsync(SmsControlInsert request);

    /// <summary>
    /// Updates an existing sms setting in the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task UpdateSmsDetailsAsync(SmsControlUpdate request);

    /// <summary>
    /// Deletes a smtp setting from the system based on its unique identifier and the user who requested the deletion.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteSmsDetailsAsync(int id, int userId);

    /// <summary>
    /// Retrieves a list of sms settings from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IListsResponse<SmsControlDetails>> GetAllSmsDetaisAsync(IListsRequest request);
    #endregion

    #region Common Status
    /// <summary>
    /// Retrieves the details of a status by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<CommonStatusDetails?> GetByStatusIdAsync(int id);

    /// <summary>
    /// Adds a new status to the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task AddStatusAsync(CommonStatusInsert request);

    /// <summary>
    /// Updates an existing status in the system based on the provided request.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task UpdateStatusAsync(CommonStatusUpdate request);

    /// <summary>
    /// Deletes a status from the system based on its unique identifier and the user who requested the deletion.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteStatusAsync(int id, int userId);

    /// <summary>
    /// Retrieves a list of statuses from the system based on the provided request parameters.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<IListsResponse<CommonStatusDetails>> GetAllStatusAsync(IListsRequest request, int userId);
    #endregion
}
