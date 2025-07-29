using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Options;
//using MilkMatrix.Admin.Business.Admin.Contracts;
//using MilkMatrix.Admin.Common.Extensions;
//using MilkMatrix.Admin.Models.Admin.Requests.User;
//using MilkMatrix.Admin.Models.Admin.Responses.User;
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
using MilkMatrix.Milk.Contracts.Price;
using MilkMatrix.Milk.Models.Request;
using MilkMatrix.Milk.Models.Request.Price;
using MilkMatrix.Milk.Models.Response;
using MilkMatrix.Milk.Models.Response.Price;

//using static MilkMatrix.Milk.Models.Constants;
using static MilkMatrix.Milk.Models.Queries.PriceQueries;

namespace MilkMatrix.Milk.Implementations.Price
{
    public class PriceService :IPriceService
    {
        private ILogging logger;

        private readonly IRepositoryFactory repositoryFactory;

        private readonly ICsvReader csvReader;
        private readonly IQueryMultipleData queryMultipleData;

        private readonly FileConfig fileConfig;

        private readonly AppConfig appConfig;

        private readonly IBulkProcessingTasks bulkProcessingTasks;

        public PriceService(IBulkProcessingTasks bulkProcessingTasks, ILogging logger, IRepositoryFactory repositoryFactory, IOptions<FileConfig> fileConfig, IQueryMultipleData queryMultipleData, IOptions<AppConfig> appConfig, ICsvReader csvReader)
        {
            this.bulkProcessingTasks = bulkProcessingTasks;
            this.logger = logger.ForContext("ServiceName", nameof(PriceService));
            this.repositoryFactory = repositoryFactory;
            this.fileConfig = fileConfig.Value ?? throw new ArgumentNullException(nameof(fileConfig), "FileConfig cannot be null");
            this.queryMultipleData = queryMultipleData;
            //this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig), "AppConfig cannot be null");
            //this.csvReader = csvReader ?? throw new ArgumentNullException(nameof(csvReader), "CSV Reader cannot be null");
        }

        public async Task AddAsync(MilkPriceInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                    { "BusinessEntityId",request.BusinessEntityId ?? (object)DBNull.Value },
                    { "WithEffectDate", request.WithEffectDate ?? (object)DBNull.Value },
                    { "ShiftId", request.ShiftId ?? (object)DBNull.Value },
                    { "MilkTypeId", request.MilkTypeId ?? (object)DBNull.Value },
                    { "RateTypeId", request.RateTypeId ?? (object)DBNull.Value },
                    { "Description", request.Description ?? (object)DBNull.Value },
                    { "RateGenType", request.RateGenType ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                    //{"PriceDetail" , request.PriceDetails ?? (object)DBNull.Value }, 
                };
                var response = await repository.AddAsync(PriceQuery.InsupdMilkPrice, requestParams, CommandType.StoredProcedure);
                // Return the inserted StateId or Name, etc. depending on your SP response
                //return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";


                var milkPriceCode = Convert.ToInt32(response);

                // 2. Loop through details and insert one-by-one
                foreach (var detail in request.PriceDetails)
                {
                    var detailParams = new Dictionary<string, object>
                    {
                        { "ActionType", (int)CrudActionType.Create}, // 1 for insert
                        { "RateCode", milkPriceCode },
                        { "FAT", detail.Fat ?? (object)DBNull.Value },
                        { "Price", detail.Price ?? (object)DBNull.Value },
                        { "SNF", detail.SNF ?? (object)DBNull.Value },
                        { "IsActive", request.IsActive ?? (object)DBNull.Value },
                        { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value },
                    };
                      await repository.AddAsync(PriceQuery.InsupdMilkPriceDetail, detailParams, CommandType.StoredProcedure);
                    //await repository.AddAsync("InsertMilkPriceDetail", detailParams, CommandType.StoredProcedure);
                }

                logger.LogInfo($"Milk Price {request.WithEffectDate} added with {request.PriceDetails?.Count ?? 0} detail records.");



                logger.LogInfo($"Milk Price {request.WithEffectDate} added successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in AddAsync for Milk Price: {request.WithEffectDate}", ex);
                throw;
            }
        }

        public async Task UpdateAsync(MilkPriceUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "RateCode", request.RateCode},
                    { "BusinessEntityId",request.BusinessEntityId ?? (object)DBNull.Value },
                    { "WithEffectDate", request.WithEffectDate ?? (object)DBNull.Value },
                    { "ShiftId", request.ShiftId ?? (object)DBNull.Value },
                    { "MilkTypeId", request.MilkTypeId ?? (object)DBNull.Value },
                    { "RateTypeId", request.RateTypeId ?? (object)DBNull.Value },
                    { "Description", request.Description ?? (object)DBNull.Value },
                    { "RateGenType", request.RateGenType ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy }
                };

                await repository.UpdateAsync(PriceQuery.InsupdMilkPrice, requestParams, CommandType.StoredProcedure);

                logger.LogInfo($"Milk Price {request.WithEffectDate} updated successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in UpdateAsync for Milk Price: {request.WithEffectDate}", ex);
                throw;
            }
        }

        public async Task DeleteAsync(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType" , (int)CrudActionType.Delete },
                    {"RateCode", id },
                    {"IsActive", false },
                    {"ModifyBy", userId }

                };

                var response = await repository.DeleteAsync(PriceQuery.InsupdMilkPrice, requestParams, CommandType.StoredProcedure);

                logger.LogInfo($"Milk Price with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError($"Error in DeleteAsync for Milk Price id: {id}", ex);
                throw;
            }

        }

        public async Task<IListsResponse<MilkPriceInsertResponse>> GetAllAsync(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All },
                { "Start", request.Limit },
                { "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<MilkPriceInsertResponse, int, FiltersMeta>(PriceQuery.MilkPriceList,
                    DbConstants.Main, parameters, null);

            // 2. Build criteria from client request and filter meta
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            // 3. Apply filtering, sorting, and paging
            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            // 4. Get count after filtering (before paging)
            var filteredCount = filtered.Count();

            // 5. Return result
            return new ListsResponse<MilkPriceInsertResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<MilkPriceInsertResponse?> GetByIdAsync(int id)
        {
            try
            {
                logger.LogInfo($"GetByIdAsync called for Milk Price id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<MilkPriceInsertResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<MilkPriceInsertResponse>(PriceQuery.MilkPriceList, new Dictionary<string, object> {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "RateCode", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new MilkPriceInsertResponse();
                logger.LogInfo(result != null
                    ? $"Milk Price with id {id} retrieved successfully."
                    : $"Milk Price with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in GetByIdAsync for Milk Price id: {id}", ex);
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
            var result = await csvReader.ReadCsvFile<BulkMilkPriceUploadRequest>(bytes);
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
                    logger.LogInfo("Bulk Milk Price upload completed in background.");
                    // TODO: Add SignalR notification here if needed
                }
                catch (Exception ex)
                {
                    logger.LogError("Error in background bulk Milk Price upload", ex);
                    // TODO: Optionally notify user of failure
                }
            });
        }

        private async Task AddBulkUsersToStagingAsync(IEnumerable<BulkMilkPriceUploadRequest> requests, int userId)
        {
            if (requests == null || !requests.Any())
            {
                logger.LogWarning("No user requests provided for bulk insert.");
                return;
            }
            logger.LogInfo($"AddBulkMilkPriceToStagingAsync called with {requests.Count()} requests.");

            //foreach (var user in requests.Where(user => string.IsNullOrWhiteSpace(user.Password)))
            //{
            //    user.Password = appConfig.DefaultPassword.EncodeSHA512();
            //    user.CreatedBy = userId;
            //}

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

            var repo = repositoryFactory.ConnectDapper<BulkMilkPriceUploadRequest>(DbConstants.Main);
            if (repo == null)
                throw new InvalidOperationException("Repository is not present");
            try
            {
                await repo.BulkInsertAsync(PriceQuery.UserStagingTable, requests, propsMapping, appConfig.UserBulkUploadBatchSize);

                // Call the processing stored procedure
                await repo.ExecuteScalarAsync(PriceQuery.ProcessStagedUsers, null, CommandType.StoredProcedure);

                // Optionally, fetch failed records for reporting
                var failedRecords = await repo.QueryAsync<BulkMilkPriceUploadRequest>(PriceQuery.GetFailedBulkProcessingUsers, null, null, CommandType.Text);

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

    }
}
