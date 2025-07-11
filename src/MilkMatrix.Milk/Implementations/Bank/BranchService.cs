using System.Data;
using Azure.Core;
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
using MilkMatrix.Milk.Contracts.Bank;
using MilkMatrix.Milk.Models.Request.Bank;
using MilkMatrix.Milk.Models.Response.Bank;
using static MilkMatrix.Milk.Models.Queries.BankQueries;
namespace MilkMatrix.Milk.Implementations
{
    public class BranchService : IBranchService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public BranchService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(DistrictService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }
        public async Task AddBranch(BranchInsertRequest request)
        {
            var repo = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
        {
            {"ActionType", (int)CrudActionType.Create},
            {"BranchCode", request.BranchCode },
            {"BankID", request.BankID },
            {"BranchName", request.BranchName },
            {"LocalBranchName", request.LocalBranchName ?? (object)DBNull.Value },
            {"IFSC", request.IFSC },
            {"StateID", request.StateID },
            {"DistrictID", request.DistrictID },
            {"TehsilID", request.TehsilID },
            {"VillageID", request.VillageID },
            {"HamletID", request.HamletID },
            {"Address", request.Address ?? (object)DBNull.Value },
            {"AddressHindi", request.AddressHindi ?? (object)DBNull.Value },
            {"Pincode", request.Pincode ?? (object)DBNull.Value },
            {"ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
            {"ContactNo", request.ContactNo ?? (object)DBNull.Value },
            {"IsStatus", request.IsActive },
            {"CreatedBy", request.CreatedBy },
        };
            await repo.AddAsync(BranchQueries.AddBranch, requestParams, CommandType.StoredProcedure);
            logging.LogInfo($"Branch {request.BranchName} added successfully.");
        }

        public async Task UpdateBranch(BranchUpdateRequest request)
        {
            var repo = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
        {
            {"ActionType", (int)CrudActionType.Update},
            {"BranchID", request.BranchID },
            {"BranchCode", request.BranchCode },
            {"BankID", request.BankID },
            {"BranchName", request.BranchName },
            {"LocalBranchName", request.LocalBranchName ?? (object)DBNull.Value },
            {"IFSC", request.IFSC },
            {"StateID", request.StateID },
            {"DistrictID", request.DistrictID },
            {"TehsilID", request.TehsilID },
            {"VillageID", request.VillageID },
            {"HamletID", request.HamletID },
            {"Address", request.Address ?? (object)DBNull.Value },
            {"AddressHindi", request.AddressHindi ?? (object)DBNull.Value },
            {"Pincode", request.Pincode ?? (object)DBNull.Value },
            {"ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
            {"ContactNo", request.ContactNo ?? (object)DBNull.Value },
            {"IsStatus", request.IsActive ?? (object)DBNull.Value },
            {"ModifiedBy", request.ModifiedBy ?? (object)DBNull.Value },
        };
            await repo.UpdateAsync(BranchQueries.AddBranch, requestParams, CommandType.StoredProcedure);
            logging.LogInfo($"Branch {request.BranchName} updated successfully.");
        }

        public async Task Delete(int id, int userId)
        {
            var repo = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
        {
            {"ActionType", (int)CrudActionType.Delete},
            {"BranchID", id },
            {"DeletedBy", userId }
        };
            await repo.DeleteAsync(BranchQueries.AddBranch, requestParams, CommandType.StoredProcedure);
            logging.LogInfo($"Branch with id {id} deleted successfully.");
        }

        public async Task<BranchResponse?> GetByBranchId(int id)
        {
            var repo = repositoryFactory.ConnectDapper<BranchResponse>(DbConstants.Main);
            var result = await repo.QueryAsync<BranchResponse>(BranchQueries.GetBranch, new Dictionary<string, object>
                {
                    {"ActionType", 2 },
                    {"BranchID", id },
                    {"IsStatus", true }
                }, null);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<BranchResponse>> GetBranches(BranchRequest request)
        {
            var repo = repositoryFactory.Connect<BranchResponse>(DbConstants.Main);
            var result = await repo.QueryAsync<BranchResponse>(BranchQueries.GetBranch, new Dictionary<string, object>
        {
            {"ActionType", (int)request.ActionType },
            {"BranchID", request.BranchID },
            {"IsStatus", request.IsActive ?? true }
        }, null, CommandType.StoredProcedure);
            return result;
        }

        public async Task<IListsResponse<BranchResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<BranchResponse, int, FiltersMeta>(BranchQueries.GetBranchList,
                    DbConstants.Main, parameters, null);

            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);

            return new ListsResponse<BranchResponse>
            {
                Count = filtered.Count(),
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }

        public Task<IEnumerable<CommonLists>> GetSpecificLists(BranchRequest request)
        {
            throw new NotImplementedException();
        }

    }
}
