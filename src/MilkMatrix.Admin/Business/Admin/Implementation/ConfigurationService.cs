using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Models.Admin.Requests.Business;
using MilkMatrix.Admin.Models.Admin.Requests.ConfigurationSettings;
using MilkMatrix.Admin.Models.Admin.Responses.ConfigurationSettings;
using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Entities.Response.Business;
using MilkMatrix.Core.Extensions;
using static MilkMatrix.Admin.Models.Constants;

namespace MilkMatrix.Admin.Business.Admin.Implementation
{
    public class ConfigurationService : IConfigurationService
    {
        private ILogging logger;

        private readonly IRepositoryFactory repositoryFactory;

        private readonly IQueryMultipleData queryMultipleData;

        private readonly AppConfig appConfig;

        public ConfigurationService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<AppConfig> appConfig, IQueryMultipleData queryMultipleData)
        {
            this.logger = logger.ForContext("ServiceName", nameof(ConfigurationService));
            this.repositoryFactory = repositoryFactory;
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
            this.queryMultipleData = queryMultipleData ?? throw new ArgumentNullException(nameof(queryMultipleData), "QueryMultipleData cannot be null");
        }

        public async Task AddAsync(ConfigurationInsertRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request cannot be null");

            try
            {
                logger.LogInfo($"AddAsync called for configuration: {request.TagName}");

                var repo = repositoryFactory.ConnectDapper<ConfigurationInsertRequest>(DbConstants.Main);

                var parameters = new Dictionary<string, object>
                {
                    ["TagName"] = request.TagName,
                    ["TagValue"] = request.TagValue,
                    ["TagValueBool"] = request.TagValueBool,
                    ["TagFlag"] = request.TagFlag,
                    ["SkipForUser"] = request.SkipForUser,
                    ["BusinessId"] = request.BusinessId,
                    ["DeviceType"] = request.DeviceType,
                    ["CreatedBy"] = request.CreatedBy,
                    ["ActionType"] = (int)CrudActionType.Create
                };

                await repo.AddAsync(ConfigurationSettingSpName.ConfigurationUpsert, parameters);
                logger.LogInfo($"Configuration {request.TagName} added successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        public async Task DeleteAsync(int id, int userId)
        {
            try
            {
                logger.LogInfo($"DeleteAsync called for Configuration id: {id}");
                var repo = repositoryFactory.ConnectDapper<ConfigurationDetails>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
            {
                {"Id", id },
                {"Status", false },
                {"ModifyBy", userId },
                {"ActionType" , (int)CrudActionType.Delete }
            };
                await repo.DeleteAsync(ConfigurationSettingSpName.ConfigurationUpsert, parameters);
                logger.LogInfo($"Configuration with id {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in DeleteAsync for Configuration id: {id}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<ConfigurationDetails>> GetAllAsync(IListsRequest request, int userId)
        {
            var repo = repositoryFactory
           .ConnectDapper<UserDetails>(DbConstants.Main);
            var user = (await repo.QueryAsync<UserDetails>(AuthSpName.LoginUserDetails, new Dictionary<string, object> { { "UserId", userId } }, null))!.FirstOrDefault()!;

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<ConfigurationDetails, int, FiltersMeta>(BusinessSpName.GetBusinessDetails,
                DbConstants.Main,
                new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.All },
                    { "Status", true },
                    { "BusinessId", user.UserType == 0 ? default : user.BusinessId } },
                null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<ConfigurationDetails>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<ConfigurationDetails?> GetByIdAsync(int id)
        {
            try
            {
                logger.LogInfo($"GetByIdAsync called for Configuration id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<ConfigurationDetails>(DbConstants.Main);
                var data = await repo.QueryAsync<ConfigurationDetails>(ConfigurationSettingSpName.GetConfigurationSettings, new Dictionary<string, object> { { "Id", id },
                                                                                { "ActionType", (int)ReadActionType.Individual } }, null);

                var result = data.Any() ? data.FirstOrDefault() : default;
                logger.LogInfo(result != null
                    ? $"Configuration with id {id} retrieved successfully."
                    : $"Configuration with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in GetByIdAsync for Configuration id: {id}", ex);
                throw;
            }
        }

        public async Task UpdateAsync(ConfigurationUpdateRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request cannot be null");

            try
            {
                logger.LogInfo($"UpdateAsync called for configuration: {request.TagName}");

                var repo = repositoryFactory.ConnectDapper<ConfigurationInsertRequest>(DbConstants.Main);

                var parameters = new Dictionary<string, object>
                {
                    ["Id"] = request.Id,
                    ["TagName"] = request.TagName,
                    ["TagValue"] = request.TagValue,
                    ["TagValueBool"] = request.TagValueBool,
                    ["TagFlag"] = request.TagFlag,
                    ["SkipForUser"] = request.SkipForUser,
                    ["BusinessId"] = request.BusinessId,
                    ["DeviceType"] = request.DeviceType,
                    ["ModifyBy"] = request.ModifyBy,
                    ["ActionType"] = (int)CrudActionType.Update
                };

                await repo.AddAsync(ConfigurationSettingSpName.ConfigurationUpsert, parameters);
                logger.LogInfo($"Configuration {request.TagName} updated successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
