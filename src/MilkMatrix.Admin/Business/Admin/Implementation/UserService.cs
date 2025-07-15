using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MilkMatrix.Admin.Business.Admin.Contracts;
using MilkMatrix.Admin.Common.Extensions;
using MilkMatrix.Admin.Models.Admin.Requests.User;
using MilkMatrix.Admin.Models.Admin.Responses.User;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Common;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using static MilkMatrix.Admin.Models.Constants;

/// <summary>
/// Service for managing user-related operations such as adding, updating, deleting, and retrieving user details.
/// </summary>
public class UserService : IUserService
{
    private ILogging logger;

    private readonly IRepositoryFactory repositoryFactory;

    private readonly IQueryMultipleData queryMultipleData;

    private readonly FileConfig fileConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="repositoryFactory"></param>
    /// <param name="appConfig"></param>
    /// <param name="queryMultipleData"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UserService(ILogging logger, IRepositoryFactory repositoryFactory, IOptions<FileConfig> fileConfig, IQueryMultipleData queryMultipleData)
    {
        this.logger = logger.ForContext("ServiceName", nameof(UserService));
        this.repositoryFactory = repositoryFactory;
        this.fileConfig = fileConfig.Value ?? throw new ArgumentNullException(nameof(fileConfig), "FileConfig cannot be null");
        this.queryMultipleData = queryMultipleData;
    }

    ///<inheritdoc />
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
        var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters);

        // 2. Build global search fields
        var globalFields = new[] { "username", "emailid", "mobileno" }; // Add more as needed

        // 3. Apply global search if request.Search is not null/empty
        var query = allResults.WithFullPath(fileConfig.UploadFileHost).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            // Build a FilterCriteria for global search
            var globalFilter = new FilterCriteria
            {
                Property = Constants.SearchString,
                Operator = "contains",
                Value = request.Filters
            };

            // Insert globalFilter at the start of filters
            filters = filters?.ToList() ?? new List<FilterCriteria>();
            filters.Insert(0, globalFilter);
        }

        // 4. Apply filters (global + field-specific)
        var filtered = query.ApplyFilters(filters, globalFields);

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
}
