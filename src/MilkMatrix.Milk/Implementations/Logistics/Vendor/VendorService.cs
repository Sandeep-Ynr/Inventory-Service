using System.Data;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.DataProvider;
using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Infrastructure.Common.DataAccess.Dapper;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Response.Logistics.Vendor;
using MilkMatrix.Milk.Models.Request.Logistics.Vendor;
using MilkMatrix.Milk.Contracts.Logistics.Vendor;

namespace MilkMatrix.Milk.Implementations
{
    public class VendorService : IVendorService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public VendorService(ILogging logging, IOptions<AppConfig> appConfig,
                             IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(VendorService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddVendor(VendorInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "VendorCode", request.VendorCode ?? (object)DBNull.Value },
                    { "VendorName", request.VendorName ?? (object)DBNull.Value },
                    { "ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
                    { "MobileNo", request.MobileNo ?? (object)DBNull.Value },
                    { "Email", request.Email ?? (object)DBNull.Value },
                    { "GSTIN", request.GSTIN ?? (object)DBNull.Value },
                    { "PanNo", request.PanNo ?? (object)DBNull.Value },
                    { "Address", request.Address ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus?? (object)DBNull.Value  },
                    { "CreatedBy", request.CreatedBy?? (object)DBNull.Value  }
                };

                var message = await repository.AddAsync(VendorQueries.AddVendor, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Vendor {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddVendor: {request.VendorCode}", ex);
                throw;
            }
        }

   
        public async Task UpdateVendor(VendorUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "VendorCode", request.VendorCode ?? (object)DBNull.Value  },
                    { "VendorName", request.VendorName  ?? (object)DBNull.Value },
                    { "ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
                    { "MobileNo", request.MobileNo ?? (object)DBNull.Value },
                    { "Email", request.Email ?? (object)DBNull.Value },
                    { "GSTIN", request.GSTIN ?? (object)DBNull.Value },
                    { "PanNo", request.PanNo ?? (object)DBNull.Value },
                    { "Address", request.Address ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus  ?? (object)DBNull.Value },
                    { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value }
                };

                var message = await repository.UpdateAsync(VendorQueries.AddVendor, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Vendor '{request.VendorName}' updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateVendor: {request.VendorName}", ex);
                throw;
            }
        }
   

    

      public async Task<IListsResponse<VendorResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<VendorResponse, int, FiltersMeta>(VendorQueries.GetVendorList,
                    DbConstants.Main, parameters, null);

            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);
            var filteredCount = filtered.Count();

            return new ListsResponse<VendorResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public async Task<VendorResponse?> GetByVendorId(int vendorId)
        {
            try
            {
                logging.LogInfo($"GetByCodeAsync called for Vendor Id: {vendorId}");
                var repo = repositoryFactory.ConnectDapper<VendorResponse>(DbConstants.Main);

                var data = await repo.QueryAsync<VendorResponse>(VendorQueries.GetVendorList,
                    new Dictionary<string, object>
                    {
                        { "ActionType", (int)ReadActionType.Individual },
                        { "VendorId", vendorId }
                    }, null);

                var result = data.FirstOrDefault() ?? new VendorResponse();
                logging.LogInfo(result != null
                    ? $"Vendor with Id {vendorId} retrieved successfully."
                    : $"Vendor with Id {vendorId} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByCodeAsync for Vendor Code: {vendorId}", ex);
                throw;
            }
        }

        public async Task DeleteVendor(int vendorId, long userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "VendorID", vendorId },
                    { "ModifyBy", userId }
                };

                await repository.DeleteAsync(VendorQueries.AddVendor, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Vendor '{vendorId}' deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteVendor for Code: {vendorId}", ex);
                throw;
            }
        }
    }
}
