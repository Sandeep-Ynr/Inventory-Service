using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Requests.Role;
using MilkMatrix.Admin.Models.Admin.Responses.Role;
using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Domain.Entities.Enums;
using MilkMatrix.Infrastructure.Models.Config;
using static MilkMatrix.Admin.Models.Constants;

namespace MilkMatrix.Admin.Business.Admin.Implementation;

public class RoleService : IRoleService
{
    private ILogging logger;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly AppConfig appConfig;
    public RoleService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig)
    {
        this.logger = logger.ForContext("ServiceName", nameof(RoleService));
        this.repositoryFactory = repositoryFactory;
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
    }

    public async Task AddAsync(RoleInsertRequest request)
    {
        try
        {
            logger.LogInfo($"AddAsync called for role: {request.RoleName}");
            var repo = repositoryFactory.ConnectDapper<UserDetails>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["RoleName"] = request.RoleName,
                ["BusinessId"] = request.BusinessId,
                ["CreatedBy"] = request.CreatedBy,
                ["Status"] = true,
                ["ActionType"] = (int)CrudActionType.Create
            };
            await repo.AddAsync(RoleSpName.RoleUpsert, parameters);
            logger.LogInfo($"Role {request.RoleName} added successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in AddAsync for user: {request.RoleName}", ex);
            throw;
        }
    }

    public async Task DeleteAsync(int id, int userId)
    {
        try
        {
            logger.LogInfo($"DeleteAsync called for role id: {id}");
            var repo = repositoryFactory.ConnectDapper<Roles>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                {"RoleID", id },
                {"Status", false },
                {"ModifyBy", userId },
                {"ActionType" , (int)CrudActionType.Delete }
            };
            await repo.DeleteAsync(RoleSpName.RoleUpsert, parameters);
            logger.LogInfo($"Role with id {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in DeleteAsync for role id: {id}", ex);
            throw;
        }
    }

    public async Task<Roles?> GetByIdAsync(int id)
    {
        try
        {
            logger.LogInfo($"GetByIdAsync called for role id: {id}");
            var repo = repositoryFactory
                       .ConnectDapper<Roles>(DbConstants.Main);
            var data = await repo.QueryAsync<Roles>(RoleSpName.GetRoles, new Dictionary<string, object> { { "RoleId", id },
                                                                                { "ActionType", (int)ReadActionType.Individual } }, null);

            var result = data.Any() ? data.FirstOrDefault() : default;
            logger.LogInfo(result != null
                ? $"Role with id {id} retrieved successfully."
                : $"Role with id {id} not found.");
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetByIdAsync for user id: {id}", ex);
            throw;
        }
    }

    public async Task UpdateAsync(RoleUpdateRequest request)
    {
        try
        {
            logger.LogInfo($"UpdateAsync called for user: {request.RoleName}");
            var repo = repositoryFactory.ConnectDapper<UserDetails>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["RoleID"] = request.RoleId,
                ["RoleName"] = request.RoleName,
                ["BusinessId"] = request.BusinessId,
                ["ModifyBy"] = request.ModifyBy,
                ["Status"] = request.IsActive,
                ["ActionType"] = (int)CrudActionType.Update
            };
            await repo.UpdateAsync(UserSpName.UserUpsert, parameters);
            logger.LogInfo($"Role {request.RoleName} updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in UpdateAsync for role: {request.RoleName}", ex);
            throw;
        }
    }

    public Task<IEnumerable<Roles>> GetAllAsync(Dictionary<string, object> filters)
    {
        throw new NotImplementedException();
    }
}
