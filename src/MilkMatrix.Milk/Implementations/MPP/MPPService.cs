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
using MilkMatrix.Milk.Contracts.MPP;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.MPP;
using MilkMatrix.Milk.Models.Response.MPP;
namespace MilkMatrix.Milk.Implementations
{
    public class MPPService : IMPPService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public MPPService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(MPPService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddMPP(MPPInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "Code", request.Code },
                    { "CompanyCode", request.CompanyCode },
                    { "MPPName", request.MPPName },
                    { "BmcId", request.BmcId },
                    { "RouteID", request.RouteID },
                    { "ShortName", request.ShortName ?? (object)DBNull.Value },
                    { "RegionalName", request.RegionalName ?? (object)DBNull.Value },
                    { "MPPExCode", request.MPPExCode },
                    { "RegistrationNo", request.RegistrationNo ?? (object)DBNull.Value },
                    { "RegistrationDate", request.RegistrationDate ?? (object)DBNull.Value },
                    { "Logo", request.Logo ?? (object)DBNull.Value },
                    { "PunchLine", request.PunchLine ?? (object)DBNull.Value },
                    { "StateID", request.StateID },
                    { "DistrictID", request.DistrictID },
                    { "TehsilID", request.TehsilID },
                    { "VillageID", request.VillageID },
                    { "HamletID", request.HamletID },
                    { "Address", request.Address ?? (object)DBNull.Value },
                    { "RegionalAddress", request.RegionalAddress ?? (object)DBNull.Value },
                    { "Pincode", request.Pincode ?? (object)DBNull.Value },
                    { "MobileNo", request.MobileNo ?? (object)DBNull.Value },
                    { "PhoneNo", request.PhoneNo ?? (object)DBNull.Value },
                    { "ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
                    { "ContactRegionalName", request.ContactRegionalName ?? (object)DBNull.Value },
                    { "Pancard", request.Pancard ?? (object)DBNull.Value },
                    { "BankID", request.BankID },
                    { "BranchID", request.BranchID },
                    { "Business_entity_id", request.Business_entity_id ?? 0 },
                    { "AccNo", request.AccNo ?? (object)DBNull.Value },
                    { "IFSC", request.IFSC ?? (object)DBNull.Value },
                    { "NoOfVillageMapped", request.NoOfVillageMapped ?? (object)DBNull.Value },
                    { "PouringMethod", request.PouringMethod ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus ?? true },
                    { "CreatedBy", request.CreatedBy ?? 0 }
                };

             var message = await repository.AddAsync("usp_mppmaster_insupd", requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"MPP {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddMPP: {request.MPPName}", ex);
                throw;
            }
        }

        public async Task UpdateMPP(MPPUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "MPPID", request.MPPID },
                    { "Code", request.Code },
                    { "CompanyCode", request.CompanyCode },
                    { "MPPName", request.MPPName },
                    { "BmcId", request.BmcId },
                    { "RouteID", request.RouteID },
                    { "ShortName", request.ShortName ?? (object)DBNull.Value },
                    { "RegionalName", request.RegionalName ?? (object)DBNull.Value },
                    { "MPPExCode", request.MPPExCode },
                    { "RegistrationNo", request.RegistrationNo ?? (object)DBNull.Value },
                    { "RegistrationDate", request.RegistrationDate ?? (object)DBNull.Value },
                    { "Logo", request.Logo ?? (object)DBNull.Value },
                    { "PunchLine", request.PunchLine ?? (object)DBNull.Value },
                    { "StateID", request.StateID },
                    { "DistrictID", request.DistrictID },
                    { "TehsilID", request.TehsilID },
                    { "VillageID", request.VillageID },
                    { "HamletID", request.HamletID },
                    { "Address", request.Address ?? (object)DBNull.Value },
                    { "RegionalAddress", request.RegionalAddress ?? (object)DBNull.Value },
                    { "Pincode", request.Pincode ?? (object)DBNull.Value },
                    { "MobileNo", request.MobileNo ?? (object)DBNull.Value },
                    { "PhoneNo", request.PhoneNo ?? (object)DBNull.Value },
                    { "ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
                    { "ContactRegionalName", request.ContactRegionalName ?? (object)DBNull.Value },
                    { "Pancard", request.Pancard ?? (object)DBNull.Value },
                    { "BankID", request.BankID },
                    { "BranchID", request.BranchID },
                    { "AccNo", request.AccNo ?? (object)DBNull.Value },
                    { "IFSC", request.IFSC ?? (object)DBNull.Value },
                    { "NoOfVillageMapped", request.NoOfVillageMapped ?? (object)DBNull.Value },
                    { "PouringMethod", request.PouringMethod ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus },
                    { "Business_entity_id", request.Business_entity_id ?? (object)DBNull.Value },
                    { "ModifiedBy", request.ModifiedBy ?? 0 }
                };
                var message = await repository.UpdateAsync(MPPQueries.AddMPP, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"MPP {request.MPPName} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateMPP: {request.MPPName}", ex);
                throw;
            }
        }


        public async Task Delete(int id, int userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType" , (int)CrudActionType.Delete },
                    {"MPPID", id },
                };

                var response = await repository.DeleteAsync(
                   MPPQueries.AddMPP, requestParams, CommandType.StoredProcedure
                );

                logging.LogInfo($"MPP Type with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for MPP id: {id}", ex);
                throw;
            }
        }

        public async Task<IListsResponse<MPPResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>() {
                { "ActionType", (int)ReadActionType.All }
                //{ "Start", request.Limit },
                //{ "End", request.Offset }
            };

            // 1. Fetch all results, count, and filter meta from stored procedure
            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<MPPResponse, int, FiltersMeta>(MPPQueries.GetMPPList,
                    DbConstants.Main,
                    parameters,
                    null);

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
            return new ListsResponse<MPPResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
        

        public async Task<MPPResponse?> GetById(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for MPP id: {id}");
                var repo = repositoryFactory
                           .ConnectDapper<MPPResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<MPPResponse>(MPPQueries.GetMPPList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "MPPID", id }
                }, null);

                var result = data.Any() ? data.FirstOrDefault() : new MPPResponse();
                logging.LogInfo(result != null
                    ? $"MPPR with id {id} retrieved successfully."
                    : $"MPPR with id {id} not found.");
                return result;
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for MPPR id: {id}", ex);
                throw;
            }
        }

        public Task<IEnumerable<MPPResponse>> GetMPP(MPPRequest request)
        {
            throw new NotImplementedException();
        }

      
    }
}

