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
using MilkMatrix.Milk.Contracts.Party;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Party;
using MilkMatrix.Milk.Models.Response.Party;

namespace MilkMatrix.Milk.Implementations
{
    public class PartyGroupService : IPartyGroupService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public PartyGroupService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(PartyGroupService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddPartyGroup(PartyGroupInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "GroupCode", request.GroupCode },
                    { "GroupName", request.GroupName },
                    { "GroupShortName", request.GroupShortName ?? (object)DBNull.Value },
                    { "IsStatus", request.IsActive },
                    { "CreatedBy", request.CreatedBy }
                };

                var message = await repository.AddAsync(PartyQueries.AddPartyGroup, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Party {message} added successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddPartyGroup: {request.GroupName}", ex);
                throw;
            }
        }

        public async Task UpdatePartyGroup(PartyGroupUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "GroupId", request.GroupId },
                    { "GroupCode", request.GroupCode },
                    { "GroupName", request.GroupName },
                    { "GroupShortName", request.GroupShortName ?? (object)DBNull.Value },
                    { "IsStatus", request.IsActive },
                    { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value }
                };

                await repository.UpdateAsync(PartyQueries.AddPartyGroup, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Party Group {request.GroupName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateAsync for Party Group: {request.GroupName}", ex);
                throw;
            }
        }
        public async Task DeleteById(long id, long userId)
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)CrudActionType.Delete },
                { "GroupId", id },
                { "ModifyBy", userId }
            };
            await repository.DeleteAsync(PartyQueries.AddPartyGroup, requestParams, CommandType.StoredProcedure);
            logging.LogInfo($"Party Group with ID {id} deleted successfully.");
        }
        public async Task<PartyGroupResponse?> GetById(long id)
        {
            var repo = repositoryFactory.ConnectDapper<PartyGroupResponse>(DbConstants.Main);
            var data = await repo.QueryAsync<PartyGroupResponse>(PartyQueries.GetPartyGroupList, new Dictionary<string, object>
            {
                { "ActionType", (int)ReadActionType.Individual },
                { "GroupId", id }
            }, null);

            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<PartyGroupResponse>> GetPartyGroups(PartyGroupRequest request)
        {
            var repository = repositoryFactory.Connect<PartyGroupResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                { "GroupId", request.GroupId ?? 0 },
                { "ActionType", (int)request.ActionType },
                { "IsStatus", request.IsStatus }
            };
            return await repository.QueryAsync<PartyGroupResponse>(PartyQueries.GetPartyGroupList, requestParams, null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(PartyGroupRequest request)
        {
            var repository = repositoryFactory.Connect<PartyGroupResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)request.ActionType },
                { "IsStatus", request.IsStatus }
            };
            return await repository.QueryAsync<CommonLists>(PartyQueries.GetPartyGroupList, requestParams, null, CommandType.StoredProcedure);
        }

        public async Task<IListsResponse<PartyGroupResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object> {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<PartyGroupResponse, int, FiltersMeta>(PartyQueries.GetPartyGroupList,
                    DbConstants.Main,
                    parameters,
                    null);

            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);
            var filteredCount = filtered.Count();

            return new ListsResponse<PartyGroupResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
