using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Common.Extensions;
using MilkMatrix.Admin.Models.Admin.Requests;
using MilkMatrix.Admin.Models.Admin.Responses;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Domain.Entities.Enums;
using MilkMatrix.Infrastructure.Models.Config;
using static MilkMatrix.Admin.Models.Constants;

public class UserService : IUserService
{
    private ILogging logger;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly AppConfig appConfig;
    public UserService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig)
    {
        this.logger = logger.ForContext("ServiceName", nameof(UserService));
        this.repositoryFactory = repositoryFactory;
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
    }

    public async Task AddAsync(UserInsertRequest request)
    {
        try
        {
            logger.LogInfo($"AddAsync called for user: {request.Username}");
            var repo = repositoryFactory.ConnectDapper<UserDetails>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["Username"] = request.Username,
                ["Password"] = request.Password,
                ["EmailId"] = request.EmailId,
                ["HrmsCode"] = request.HrmsCode,
                ["RoleId"] = request.RoleId,
                ["BusinessId"] = request.BusinessId,
                ["ReportingId"] = request.ReportingId,
                ["UserType"] = request.UserType,
                ["ImageId"] = request.ImageId,
                ["MobileNumber"] = request.MobileNumber,
                ["CreatedBy"] = request.CreatedBy,
                ["Status"] = true,
                ["ActionType"] = (int)CrudActionType.Create
            };
            await repo.AddAsync(UserSpName.UserUpsert, parameters);
            logger.LogInfo($"User {request.Username} added successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in AddAsync for user: {request.Username}", ex);
            throw;
        }
    }

    public async Task DeleteAsync(int id, int userId)
    {
        try
        {
            logger.LogInfo($"DeleteAsync called for user id: {id}");
            var repo = repositoryFactory.ConnectDapper<UserDetails>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                {"Id", id },
                {"Status", false },
                {"ModifyBy", userId },
                {"ActionType" , (int)CrudActionType.Delete }
            };
            await repo.DeleteAsync(UserSpName.UserUpsert, parameters);
            logger.LogInfo($"User with id {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in DeleteAsync for user id: {id}", ex);
            throw;
        }
    }

    public async Task<UserDetails?> GetByIdAsync(int id)
    {
        try
        {
            logger.LogInfo($"GetByIdAsync called for user id: {id}");
            var repo = repositoryFactory
                       .ConnectDapper<UserDetails>(DbConstants.Main);
            var data = await repo.QueryAsync<UserDetails>(AuthSpName.LoginUserDetails, new Dictionary<string, object> { { "UserId", id } }, null);

            var result = data.Any() ? data.FirstOrDefault()!.MaskAndEncryptUserResponse(appConfig.Base64EncryptKey) : new UserDetails();
            logger.LogInfo(result != null
                ? $"User with id {id} retrieved successfully."
                : $"User with id {id} not found.");
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetByIdAsync for user id: {id}", ex);
            throw;
        }
    }

    public async Task UpdateAsync(UserUpdateRequest request)
    {
        try
        {
            logger.LogInfo($"UpdateAsync called for user: {request.Username}");
            var repo = repositoryFactory.ConnectDapper<UserDetails>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["ID"] = request.Id,
                ["Username"] = request.Username,
                ["Password"] = request.Password,
                ["EmailId"] = request.EmailId,
                ["HrmsCode"] = request.HrmsCode,
                ["RoleId"] = request.RoleId,
                ["BusinessId"] = request.BusinessId,
                ["ReportingId"] = request.ReportingId,
                ["UserType"] = request.UserType,
                ["ImageId"] = request.ImageId,
                ["MobileNumber"] = request.MobileNumber,
                ["ModifyBy"] = request.ModifyBy,
                ["Status"] = request.IsActive,
                ["ActionType"] = (int)CrudActionType.Update
            };
            await repo.UpdateAsync(UserSpName.UserUpsert, parameters);
            logger.LogInfo($"User {request.Username} updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in UpdateAsync for user: {request.Username}", ex);
            throw;
        }
    }
}
