using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Common.Extensions;
using MilkMatrix.Admin.Models.Admin.Requests.User;
using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Core.Abstractions.Csv;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.HostedServices;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.Utils;
using static MilkMatrix.Admin.Models.Constants;

/// <summary>
/// Service for managing user-related operations such as adding, updating, deleting, and retrieving user details.
/// </summary>
public class UserService : IUserService
{
    private ILogging logger;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly ICsvReader csvReader;
    private readonly IQueryMultipleData queryMultipleData;

    private readonly FileConfig fileConfig;

    private readonly AppConfig appConfig;

    private readonly IBulkProcessingTasks bulkProcessingTasks;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="repositoryFactory"></param>
    /// <param name="appConfig"></param>
    /// <param name="queryMultipleData"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UserService(IBulkProcessingTasks bulkProcessingTasks, ILogging logger, IRepositoryFactory repositoryFactory, IOptions<FileConfig> fileConfig, IQueryMultipleData queryMultipleData, IOptions<AppConfig> appConfig, ICsvReader csvReader)
    {
        this.bulkProcessingTasks = bulkProcessingTasks;
        this.logger = logger.ForContext("ServiceName", nameof(UserService));
        this.repositoryFactory = repositoryFactory;
        this.fileConfig = fileConfig.Value ?? throw new ArgumentNullException(nameof(fileConfig), "FileConfig cannot be null");
        this.queryMultipleData = queryMultipleData;
        this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
        this.csvReader = csvReader ?? throw new ArgumentNullException(nameof(csvReader), "CSV Reader cannot be null");
    }

    ///<inheritdoc />
    public async Task AddAsync(UserInsertRequest request)
    {
        try
        {
            logger.LogInfo($"AddAsync called for user: {request.Username}");
            var repo = repositoryFactory.ConnectDapper<UserInsertRequest>(DbConstants.Main);
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
                ["IsMFA"] = request.IsMFA,
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

    ///<inheritdoc />
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

    ///<inheritdoc />
    public async Task<UserDetails?> GetByIdAsync(int id)
    {
        try
        {
            logger.LogInfo($"GetByIdAsync called for user id: {id}");
            var repo = repositoryFactory
                       .ConnectDapper<UserDetails>(DbConstants.Main);
            var data = await repo.QueryAsync<UserDetails>(AuthSpName.LoginUserDetails, new Dictionary<string, object> { { "UserId", id } }, null);

            var result = data.Any() ? data.WithFullPath(fileConfig.UploadFileHost).FirstOrDefault() : new UserDetails();
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

    ///<inheritdoc />
    public async Task UpdateAsync(UserUpdateRequest request)
    {
        try
        {
            logger.LogInfo($"UpdateAsync called for user: {request.UserName}");
            var repo = repositoryFactory.ConnectDapper<UserUpdateRequest>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["ID"] = request.Id,
                ["Username"] = request.UserName,
                ["Password"] = request.Password,
                ["EmailId"] = request.EmailId,
                ["HrmsCode"] = request.HrmsCode,
                ["RoleId"] = request.RoleId,
                ["BusinessId"] = request.BusinessId,
                ["ReportingId"] = request.ReportingId,
                ["UserType"] = request.UserType,
                ["ImageId"] = request.ImageId,
                ["MobileNumber"] = request.MobileNumber,
                ["IsMFA"] = request.IsMFA,
                ["ModifyBy"] = request.ModifyBy,
                ["Status"] = request.IsActive,
                ["ActionType"] = (int)CrudActionType.Update
            };
            await repo.UpdateAsync(UserSpName.UserUpsert, parameters);
            logger.LogInfo($"User {request.UserName} updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in UpdateAsync for user: {request.UserName}", ex);
            throw;
        }
    }

    ///<inheritdoc />
    public async Task<IListsResponse<Users>> GetAllAsync(IListsRequest request, int userId)
    {
        var user = await GetByIdAsync(userId);
        var parameters = new Dictionary<string, object>() {
        { "BusinessId", user.UserType == 0 ? default : user.BusinessId } };

        var (allResults, countResult, filterMetas) = await queryMultipleData
            .GetMultiDetailsAsync<Users, int, FiltersMeta>(
                UserSpName.GetUsers,
                DbConstants.Main,
                parameters,
                null);

        // 1. Build field filters from request.Filters (Dictionary<string, object>)
        var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);

        // 3. Apply global search if request.Search is not null/empty
        var query = allResults.WithFullPath(fileConfig.UploadFileHost).AsQueryable();

        // 4. Apply filters (global + field-specific)
        var filtered = query.ApplyFilters(filters);

        // 5. Apply sorting and paging
        var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
        var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };
        var sorted = filtered.ApplySorting(sorts);
        var paged = sorted.ApplyPaging(paging);

        var filteredCount = filtered.Count();

        return new ListsResponse<Users>
        {
            Count = filteredCount,
            Results = paged.ToList(),
            Filters = filterMetas
        };
    }

    ///<inheritdoc />
    public async Task UpdateProfileAsync(UserProfileUpdate request)
    {
        try
        {
            logger.LogInfo($"UpdateProfileAsync called for user: {request.UserName}");
            var repo = repositoryFactory.ConnectDapper<UserProfileUpdate>(DbConstants.Main);
            var parameters = new Dictionary<string, object>
            {
                ["UserId"] = request.UserId,
                ["Username"] = request.UserName,
                ["ImageId"] = request.ImageId,
                ["Mobile"] = request.Mobile,
                ["ModifyBy"] = request.ModifyBy,
                ["ActionType"] = (int)CrudActionType.Update
            };
            await repo.UpdateAsync(UserSpName.UserProfileUpdate, parameters);
            logger.LogInfo($"User {request.UserName} updated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in UpdateProfileAsync for user: {request.UserName}", ex);
            throw;
        }
    }

    public async Task AddBulkUsersAsync(byte[] bytes, int userId)
    {
        if (bytes == null || bytes.Length == 0)
        {
            logger.LogWarning("No file provided for bulk user upload.");
            return;
        }
        logger.LogInfo("AddBulkUsersAsync called with a file for bulk user upload.");
        var result = await csvReader.ReadCsvFile<UserUploadRequest>(bytes);
        if (result.Errors.Any())
        {
            logger.LogError("CSV parsing errors occurred", new Exception(JsonSerializer.Serialize(result.Errors)));
            throw new InvalidOperationException("CSV parsing errors occurred");
        }

        // Enqueue the background work
        bulkProcessingTasks.QueueBulkWorkItem(async token =>
        {
            try
            {
                await AddBulkUsersToStagingAsync(result.Records, userId);
                logger.LogInfo("Bulk user upload completed in background.");
                // TODO: Add SignalR notification here if needed
            }
            catch (Exception ex)
            {
                logger.LogError("Error in background bulk user upload", ex);
                // TODO: Optionally notify user of failure
            }
        });
    }

    private async Task AddBulkUsersToStagingAsync(IEnumerable<UserUploadRequest> requests, int userId)
    {
        if (requests == null || !requests.Any())
        {
            logger.LogWarning("No user requests provided for bulk insert.");
            return;
        }
        logger.LogInfo($"AddBulkUsersToStagingAsync called with {requests.Count()} requests.");

        foreach (var user in requests.Where(user => string.IsNullOrWhiteSpace(user.Password)))
        {
            user.Password = appConfig.DefaultPassword.EncodeSHA512();
            user.CreatedBy = userId;
        }

        // Add ProcessStatus and ErrorMessage columns to mapping
        var propsMapping = new Dictionary<string, string>
                                {
                                    { "Username", "UserName" },
                                    { "Password", "Password" },
                                    { "EmailId", "Email_Id" },
                                    { "HrmsCode", "Hrms_Code" },
                                    { "RoleId", "Role_Id" },
                                    { "BusinessId", "Business_Id" },
                                    { "ReportingId", "Reporting_Id" },
                                    { "UserType", "User_Type" },
                                    { "MobileNumber", "Mobile_No" },
                                    { "IsMFA", "Is_MFA" },
                                    { "IsBulkUser", "is_bulk_user" },
                                    { "ChangePassword", "change_password" },
                                    { "IsActive", "Status" },
                                    { "CreatedBy", "Created_By" },
                                    { "ProcessStatus", "ProcessStatus" },
                                    { "ErrorMessage", "ErrorMessage" }
                                };

        var repo = repositoryFactory.ConnectDapper<UserUploadRequest>(DbConstants.Main);
        if (repo == null)
            throw new InvalidOperationException("Repository is not present");
        try
        {
            await repo.BulkInsertAsync(UserSpName.UserStagingTable, requests, propsMapping, appConfig.UserBulkUploadBatchSize);

            // Call the processing stored procedure
            await repo.ExecuteScalarAsync(UserSpName.ProcessStagedUsers, null, CommandType.StoredProcedure);

            // Optionally, fetch failed records for reporting
            var failedRecords = await repo.QueryAsync<UserUploadRequest>(UserSpName.GetFailedBulkProcessingUsers, null, null, CommandType.Text);

            if (failedRecords.Any())
            {
                logger.LogWarning($"{failedRecords.Count()} records failed to process. Check ErrorMessage column in staging table.");
                // Optionally: return or handle failedRecords as needed
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Bulk insert or processing failed", ex);
            throw;
        }
    }

    public async Task<IEnumerable<UserApprovalNotification>> GetNotificationsByIdAsync(int id)
    {
        try
        {
            logger.LogInfo($"GetByIdAsync called for user id: {id}");
            var repo = repositoryFactory
                       .ConnectDapper<UserApprovalNotification>(DbConstants.Main);
            var data = await repo.QueryAsync<UserApprovalNotification>(AuthSpName.UserApprovalNotification, new Dictionary<string, object> { { "UserId", id } }, null);

            var result = data.Any() ? data : Enumerable.Empty<UserApprovalNotification>();
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error in GetByIdAsync for user id: {id}", ex);
            throw;
        }
    }
}
