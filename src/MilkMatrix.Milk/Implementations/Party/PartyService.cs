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
    public class PartyService : IPartyService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public PartyService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(PartyService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddParty(PartyInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "GroupID", request.GroupId },
                    { "PartyCode", request.PartyCode },
                    { "PartyName", request.PartyName },
                    { "PartyEmail", request.PartyEmail ?? (object)DBNull.Value },
                    { "PartyShortName", request.PartyShortName ?? (object)DBNull.Value },
                    { "PartyAddress", request.PartyAddress ?? (object)DBNull.Value },
                    { "PartyPinCode", request.PartyPinCode ?? (object)DBNull.Value },
                    { "PartyPhoneNo", request.PartyPhoneNo ?? (object)DBNull.Value },
                    { "PartyLicenceNo", request.PartyLicenceNo ?? (object)DBNull.Value },
                    { "PartyGstNo", request.PartyGstNo ?? (object)DBNull.Value },
                    { "PartyOwnerName", request.PartyOwnerName ?? (object)DBNull.Value },
                    { "PartyOwnerEmail", request.PartyOwnerEmail ?? (object)DBNull.Value },
                    { "PartyOwnerPhoneNo", request.PartyOwnerPhoneNo ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive },
                    { "CreatedBy", request.CreatedBy }
                };
                var message = await repository.AddAsync(PartyQueries.AddParty, requestParams, CommandType.StoredProcedure);
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
                logging.LogInfo($"Party {request.PartyName} added successfully.");
                throw;
            }

        }

        public async Task UpdateParty(PartyUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "PartyID", request.PartyID },
                    { "GroupID", request.GroupId },
                    { "PartyCode", request.PartyCode },
                    { "PartyName", request.PartyName },
                    { "PartyEmail", request.PartyEmail ?? (object)DBNull.Value },
                    { "PartyShortName", request.PartyShortName ?? (object)DBNull.Value },
                    { "PartyAddress", request.PartyAddress ?? (object)DBNull.Value },
                    { "PartyPinCode", request.PartyPinCode ?? (object)DBNull.Value },
                    { "PartyPhoneNo", request.PartyPhoneNo ?? (object)DBNull.Value },
                    { "PartyLicenceNo", request.PartyLicenceNo ?? (object)DBNull.Value },
                    { "PartyGstNo", request.PartyGstNo ?? (object)DBNull.Value },
                    { "PartyOwnerName", request.PartyOwnerName ?? (object)DBNull.Value },
                    { "PartyOwnerEmail", request.PartyOwnerEmail ?? (object)DBNull.Value },
                    { "PartyOwnerPhoneNo", request.PartyOwnerPhoneNo ?? (object)DBNull.Value },
                    { "IsActive", request.IsActive },
                    { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value }
                };
                var message = await repository.AddAsync(PartyQueries.AddParty, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }
                else
                {
                    logging.LogInfo($"Party {message} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                logging.LogInfo($"Party {request.PartyName} updated successfully.");
                throw;
            }
        }

        public async Task Delete(long id, long userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Delete },
                    { "PartyID", id },
                    { "ModifyBy", userId }
                };

                await repository.DeleteAsync(PartyQueries.AddParty, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Party with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Party ID: {id}", ex);
                throw;
            }
        }

        public async Task<PartyResponse?> GetById(long id)
        {
            var repo = repositoryFactory.ConnectDapper<PartyResponse>(DbConstants.Main);
            var data = await repo.QueryAsync<PartyResponse>(PartyQueries.GetPartyList, new Dictionary<string, object>
            {
                { "ActionType", (int)ReadActionType.Individual },
                { "PartyID", id }
            }, null);

            return data.FirstOrDefault();
        }

        public async Task<IEnumerable<PartyResponse>> GetParties(PartyRequest request)
        {
            var repository = repositoryFactory.Connect<PartyResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                { "PartyID", request.PartyID ?? 0 },
                { "ActionType", (int)request.ActionType },
                { "IsStatus", request.IsStatus }
            };
            return await repository.QueryAsync<PartyResponse>(PartyQueries.GetPartyList, requestParams, null, CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(PartyRequest request)
        {
            var repository = repositoryFactory.Connect<PartyResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                { "ActionType", (int)request.ActionType },
                { "IsStatus", request.IsStatus }
            };
            return await repository.QueryAsync<CommonLists>(PartyQueries.GetPartyList, requestParams, null, CommandType.StoredProcedure);
        }

        public async Task<IListsResponse<PartyResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object> {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<PartyResponse, int, FiltersMeta>(PartyQueries.GetPartyList,
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

            return new ListsResponse<PartyResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
