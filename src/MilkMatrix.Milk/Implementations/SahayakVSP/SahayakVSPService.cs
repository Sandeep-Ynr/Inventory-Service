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
using MilkMatrix.Milk.Contracts.SahayakVSP;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.SahayakVSP;
using MilkMatrix.Milk.Models.Response.SahayakVSP;

namespace MilkMatrix.Milk.Implementations
{
    public class SahayakVSPService : ISahayakVSPService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public SahayakVSPService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(SahayakVSPService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddSahayakVSP(SahayakVSPInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "Code", request.Code },
                    { "CompanyCode", request.CompanyCode },
                    { "MPPCode", request.MPPCode },
                    { "SahayakName", request.SahayakName },
                    { "ShortName", request.ShortName ?? (object)DBNull.Value },
                    { "RegionalName", request.RegionalName ?? (object)DBNull.Value },
                    { "ExSahayakCode", request.ExSahayakCode },
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
                    { "MobileNo", request.MobileNo },
                    { "PhoneNo", request.PhoneNo ?? (object)DBNull.Value },
                    { "ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
                    { "ContactRegionalName", request.ContactRegionalName ?? (object)DBNull.Value },
                    { "Pancard", request.Pancard ?? (object)DBNull.Value },
                    { "BankID", request.BankID },
                    { "BranchID", request.BranchID },
                    { "AccNo", request.AccNo ?? (object)DBNull.Value },
                    { "IFSC", request.IFSC ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus },
                    { "CreatedBy", request.CreatedBy }
                };

                await repository.AddAsync(SahayakVSPQueries.AddSahayakVSP, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"SahayakVSP {request.SahayakName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddAsync for SahayakVSP: {request.SahayakName}", ex);
                throw;
            }
        }

        public async Task UpdateSahayakVSP(SahayakVSPUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "SahayakID", request.SahayakID },
                    { "Code", request.Code },
                    { "CompanyCode", request.CompanyCode },
                    { "MPPCode", request.MPPCode },
                    { "SahayakName", request.SahayakName },
                    { "ShortName", request.ShortName ?? (object)DBNull.Value },
                    { "RegionalName", request.RegionalName ?? (object)DBNull.Value },
                    { "ExSahayakCode", request.ExSahayakCode },
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
                    { "MobileNo", request.MobileNo },
                    { "PhoneNo", request.PhoneNo ?? (object)DBNull.Value },
                    { "ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
                    { "ContactRegionalName", request.ContactRegionalName ?? (object)DBNull.Value },
                    { "Pancard", request.Pancard ?? (object)DBNull.Value },
                    { "BankID", request.BankID },
                    { "BranchID", request.BranchID },
                    { "AccNo", request.AccNo ?? (object)DBNull.Value },
                    { "IFSC", request.IFSC ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus },
                    { "ModifyBy", request.ModifiedBy ?? (object)DBNull.Value }
                };

                await repository.UpdateAsync(SahayakVSPQueries.AddSahayakVSP, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"SahayakVSP {request.SahayakName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for SahayakVSP: {request.SahayakName}", ex);
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
                    { "ActionType", (int)CrudActionType.Delete },
                    { "SahayakID", id },
                    { "ModifyBy", userId }
                };

                await repository.DeleteAsync(SahayakVSPQueries.AddSahayakVSP, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"SahayakVSP with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for SahayakVSP ID: {id}", ex);
                throw;
            }
        }

        public async Task<SahayakVSPResponse?> GetById(int id)
        {
            try
            {
                logging.LogInfo($"GetByIdAsync called for SahayakVSP ID: {id}");
                var repo = repositoryFactory.ConnectDapper<SahayakVSPResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<SahayakVSPResponse>(SahayakVSPQueries.GetSahayakVSPList, new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "SahayakID", id }
                }, null);

                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetByIdAsync for SahayakVSP ID: {id}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<SahayakVSPResponse>> GetSahayakVSPs(SahayakVSPRequest request)
        {
            var repository = repositoryFactory.Connect<SahayakVSPResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                { "SahayakID", request.SahayakID ?? 0 },
                { "ActionType", (int)request.ActionType },
                { "IsStatus", request.IsStatus }
            };
            return await repository.QueryAsync<SahayakVSPResponse>(SahayakVSPQueries.GetSahayakVSPList, requestParams, null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(SahayakVSPRequest request)
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)request.ActionType },
                { "IsStatus", request.IsStatus }
            };
            return await repository.QueryAsync<CommonLists>(SahayakVSPQueries.GetSahayakVSPList, requestParams, null, CommandType.StoredProcedure);
        }

        public async Task<IListsResponse<SahayakVSPResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object> {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, _, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<SahayakVSPResponse, int, FiltersMeta>(SahayakVSPQueries.GetSahayakVSPList,
                    DbConstants.Main,
                    parameters,
                    null);

            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            return new ListsResponse<SahayakVSPResponse>
            {
                Count = filtered.Count(),
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

      
    }
}
