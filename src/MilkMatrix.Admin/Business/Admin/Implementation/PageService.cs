using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Requests.Page;
using MilkMatrix.Admin.Models.Admin.Responses.Page;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Domain.Entities.Enums;
using MilkMatrix.Infrastructure.Models.Config;
using static MilkMatrix.Admin.Models.Constants;

namespace MilkMatrix.Admin.Business.Admin.Implementation
{
    public class PageService : IPageService
    {
        private ILogging logger;

        private readonly IRepositoryFactory repositoryFactory;

        private readonly AppConfig appConfig;
        public PageService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig)
        {
            this.logger = logger.ForContext("ServiceName", nameof(PageService));
            this.repositoryFactory = repositoryFactory;
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
        }

        public async Task AddAsync(PageInsertRequest request)
        {
            try
            {
                logger.LogInfo($"AddAsync called for page: {request.PageName}");
                var repo = repositoryFactory.ConnectDapper<PageInsertRequest>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
                {
                    ["PageName"] = request.PageName,
                    ["PageUrl"] = request.PageUrl,
                    ["ModuleId"] = request.ModuleId,
                    ["SubModuleId"] = request.SubModuleId,
                    ["PageOrder"] = request.PageOrder,
                    ["IsMenu"] = request.IsMenu,
                    ["PageIcon"] = request.PageIcon,
                    ["ActionDetails"] = request.ActionDetails,
                    ["Status"] = true,
                    ["ActionType"] = (int)CrudActionType.Create,
                    ["CreatedBy"] = request.CreatedBy,
                };
                await repo.AddAsync(PageSpName.PageUpsert, parameters);
                logger.LogInfo($"Page {request.PageName} added successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in AddAsync for page: {request.PageName}", ex);
                throw;
            }
        }

        public async Task DeleteAsync(int id, int userId)
        {
            try
            {
                logger.LogInfo($"DeleteAsync called for role id: {id}");
                var repo = repositoryFactory.ConnectDapper<PageList>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
                {
                    ["PageId"] = id,
                    ["Status"] = false,
                    ["ActionType"] = (int)CrudActionType.Delete,
                    ["ModifyBy"] = userId,
                };
                await repo.DeleteAsync(PageSpName.PageUpsert, parameters);
                logger.LogInfo($"Page with id {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in DeleteAsync for page id: {id}", ex);
                throw;
            }
        }

        public async Task<Pages?> GetByIdAsync(int id)
        {
            try
            {
                logger.LogInfo($"GetByIdAsync called for page id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<Pages>(DbConstants.Main);
                var data = await repo.QueryAsync<Pages>(PageSpName.GetPages, new Dictionary<string, object> { { "PageId", id },
                                                                                { "ActionType", (int)ReadActionType.Individual } }, null);

                var result = data.Any() ? data.FirstOrDefault() : default;
                logger.LogInfo(result != null
                    ? $"Page with id {id} retrieved successfully."
                    : $"Page with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in GetByIdAsync for page id: {id}", ex);
                throw;
            }
        }

        public async Task UpdateAsync(PageUpdateRequest request)
        {
            try
            {
                logger.LogInfo($"UpdateAsync called for page: {request.PageName}");
                var repo = repositoryFactory.ConnectDapper<PageUpdateRequest>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
                {
                    ["PageId"] = request.PageId,
                    ["PageName"] = request.PageName,
                    ["PageUrl"] = request.PageUrl,
                    ["ModuleId"] = request.ModuleId,
                    ["SubModuleId"] = request.SubModuleId,
                    ["PageOrder"] = request.PageOrder,
                    ["IsMenu"] = request.IsMenu,
                    ["PageIcon"] = request.PageIcon,
                    ["ActionDetails"] = request.ActionDetails,
                    ["Status"] = true,
                    ["ActionType"] = (int)CrudActionType.Update,
                    ["ModifyBy"] = request.ModifyBy,
                };
                await repo.UpdateAsync(PageSpName.PageUpsert, parameters);
                logger.LogInfo($"Page {request.PageName} updated successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in UpdateAsync for page: {request.PageName}", ex);
                throw;
            }
        }

        public Task<IEnumerable<PageList>> GetAllAsync(Dictionary<string, object> filters)
        {
            throw new NotImplementedException();
        }
    }
}
