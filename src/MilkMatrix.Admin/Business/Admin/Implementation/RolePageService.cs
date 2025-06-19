using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Requests.Page;
using MilkMatrix.Admin.Models.Admin.Requests.RolePage;
using MilkMatrix.Admin.Models.Admin.Responses.Page;
using MilkMatrix.Admin.Models.Admin.Responses.Role;
using MilkMatrix.Admin.Models.Admin.Responses.RolePage;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Domain.Entities.Enums;
using MilkMatrix.Infrastructure.Models.Config;
using static MilkMatrix.Admin.Models.Constants;

namespace MilkMatrix.Admin.Business.Admin.Implementation;

public class RolePageService : IRolePageService
{
    private ILogging logger;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly AppConfig appConfig;
    public RolePageService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig)
    {
        this.logger = logger.ForContext("ServiceName", nameof(RolePageService));
        this.repositoryFactory = repositoryFactory;
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
    }

    public async Task AddAsync(RolePageInsertRequest request)
    {
        try
        {
            logger.LogInfo($"AddAsync called for page: {request.RoleId}, {request.PageId}");
            var repo = repositoryFactory.ConnectDapper<RolePageInsertRequest>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["RoleId"] = request.RoleId,
                ["PageId"] = request.PageId,
                ["BusinessId"] = request.BusinessId,
                ["ActionID"] = request.ActionDetails,
                ["ActionType"] = (int)CrudActionType.Create,
                ["CreatedBy"] = request.CreatedBy,
            };
            await repo.AddAsync(RolePageSpName.RolePageUpsert, parameters);
            logger.LogInfo($"Page {request.RoleId}, {request.PageId} added successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in AddAsync for page: {request.RoleId}, {request.PageId}", ex);
            throw;
        }
    }

    public async Task DeleteAsync(int id, int businessId, int userId)
    {
        try
        {
            logger.LogInfo($"DeleteAsync called for role id: {id}");
            var repo = repositoryFactory.ConnectDapper<RolePages>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["RoleId"] = id,
                ["BusinessId"] = businessId,
                ["ActionType"] = (int)CrudActionType.Delete,
                ["ModifyBy"] = userId,
            };
            await repo.DeleteAsync(RolePageSpName.RolePageUpsert, parameters);
            logger.LogInfo($"RolePage with id {id} deleted successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in DeleteAsync for page id: {id}", ex);
            throw;
        }
    }

    public async Task<IEnumerable<RolePages>> GetByIdAsync(int roleId, int businessId)
    {
        try
        {
            logger.LogInfo($"GetByIdAsync called for page id: {roleId}, {businessId}");
            var repo = repositoryFactory
                       .ConnectDapper<PageList>(DbConstants.Main);
            var data = await repo.QueryAsync<RolePages>(RolePageSpName.GetRolePages,
                new Dictionary<string, object> {
                    { "RoleId", roleId },
                { "BusinessId", businessId },
                { "ActionType", (int)ReadActionType.Individual} }, null);

            var result = data.Any() ? data : Enumerable.Empty<RolePages>();
            logger.LogInfo(result != null
                ? $"Page with id {roleId}, {businessId} retrieved successfully."
                : $"Page with id {roleId} , {businessId} not found.");
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetByIdAsync for page id: {roleId}, {businessId}", ex);
            throw;
        }
    }

    public async Task UpdateAsync(RolePageUpdateRequest request)
    {
        try
        {
            logger.LogInfo($"UpdateAsync called for page: {request.RoleId}, {request.PageId}");
            var repo = repositoryFactory.ConnectDapper<RolePageUpdateRequest>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["RoleId"] = request.RoleId,
                ["PageId"] = request.PageId,
                ["BusinessId"] = request.BusinessId,
                ["ActionID"] = request.ActionDetails,
                ["ActionType"] = (int)CrudActionType.Update,
                ["ModifyBy"] = request.ModifyBy,
            };
            await repo.UpdateAsync(RolePageSpName.RolePageUpsert, parameters);
            logger.LogInfo($"Role Page {request.RoleId} ,  {request.PageId} updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in UpdateAsync for role page: {request.RoleId} , {request.PageId}", ex);
            throw;
        }
    }
}
