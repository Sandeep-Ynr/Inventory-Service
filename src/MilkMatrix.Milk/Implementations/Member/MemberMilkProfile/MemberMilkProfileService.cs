using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Milk.Contracts.Member.MilkProfile;
using MilkMatrix.Milk.Models.Request.Member.MemberMilkProfile;
using MilkMatrix.Milk.Models.Response.Member.MemberMilkProfile;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MilkMatrix.Core.Abstractions.DataProvider;
using System.Data;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Extensions;
using MilkMatrix.Core.Entities.Filters;
using MilkMatrix.Core.Entities.Response;
using System.Linq;

namespace MilkMatrix.Milk.Implementations.Member.MilkProfile
{
    public class MemberMilkProfileService : IMemberMilkProfileService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public MemberMilkProfileService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(MemberMilkProfileService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddMemberMilkProfile(MemberMilkProfileInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Create},
                    {"MemberID", request.MemberID},
                    {"AnimalTypeID", request.AnimalTypeID ?? (object)DBNull.Value},
                    {"NoOfMilchAnimals", request.NoOfMilchAnimals},
                    {"AvgMilkYield", request.AvgMilkYield ?? (object)DBNull.Value},
                    {"PreferredShift", request.PreferredShift ?? (object)DBNull.Value},
                    {"PouringStartDate", request.PouringStartDate},
                    {"IsStatus", request.IsStatus ?? (object)DBNull.Value},
                    {"CreatedBy", request.CreatedBy ?? (object)DBNull.Value},
                };

                var message = await repository.AddAsync(MemberQueries.AddOrUpdateMemberMilkProfile, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Member Milk Profile for Member ID {request.MemberID} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError("Error in AddMemberMilkProfile", ex);
                throw;
            }
        }

        public async Task UpdateMemberMilkProfile(MemberMilkProfileUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Update},
                    {"MilkProfileID", request.MilkProfileID},
                    {"MemberID", request.MemberID},
                    {"AnimalTypeID", request.AnimalTypeID ?? (object)DBNull.Value},
                    {"NoOfMilchAnimals", request.NoOfMilchAnimals},
                    {"AvgMilkYield", request.AvgMilkYield ?? (object)DBNull.Value},
                    {"PreferredShift", request.PreferredShift ?? (object)DBNull.Value},
                    {"PouringStartDate", request.PouringStartDate},
                    {"IsStatus", request.IsStatus ?? (object)DBNull.Value},
                    {"ModifyBy", request.ModifiedBy ?? (object)DBNull.Value},
                };

                var message = await repository.UpdateAsync(MemberQueries.AddOrUpdateMemberMilkProfile, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Member Milk Profile with ID {request.MilkProfileID} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError("Error in UpdateMemberMilkProfile", ex);
                throw;
            }
        }

        public async Task DeleteMemberMilkProfile(long milkProfileId, long userId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Delete},
                    {"MilkProfileID", milkProfileId},
                };

                var response = await repository.DeleteAsync(MemberQueries.AddOrUpdateMemberMilkProfile, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"Member Milk Profile with ID {milkProfileId} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteMemberMilkProfile for Milk Profile ID: {milkProfileId}", ex);
                throw;
            }
        }

        public async Task<MemberMilkProfileResponse?> GetMemberMilkProfileById(long id)
        {
            try
            {
                logging.LogInfo($"GetById called for MemberMilkProfile ID: {id}");
                var repo = repositoryFactory.ConnectDapper<MemberMilkProfileResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<MemberMilkProfileResponse>(MemberQueries.GetMemberMilkProfileList, new Dictionary<string, object>
                {
                    {"ActionType", (int)ReadActionType.Individual},
                    {"MilkProfileID", id}
                }, null);

                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetMemberMilkProfileById", ex);
                throw;
            }
        }

        public async Task<IEnumerable<MemberMilkProfileResponse>> GetMemberMilkProfiles(MemberMilkProfileRequestModel request)
        {
            try
            {
                var repo = repositoryFactory.ConnectDapper<MemberMilkProfileResponse>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
                {
                    {"ActionType", (int)(request.ActionType ?? ReadActionType.All)},
                    {"MilkProfileID", request.MilkProfileID ?? (object)DBNull.Value},
                    {"MemberID", request.MemberID ?? (object)DBNull.Value},
                    {"AnimalTypeID", request.AnimalTypeID ?? (object)DBNull.Value},
                    {"PreferredShift", request.PreferredShift ?? (object)DBNull.Value},
                    {"is_status", request.is_status ?? (object)DBNull.Value},
                    {"is_deleted", request.is_deleted ?? (object)DBNull.Value}
                };

                return await repo.QueryAsync<MemberMilkProfileResponse>(MemberQueries.GetMemberMilkProfileList, parameters, null);
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetMemberMilkProfiles", ex);
                throw;
            }
        }

        public async Task<IListsResponse<MemberMilkProfileResponse>> GetAllMemberMilkProfiles(IListsRequest request)
        {
            var parameters = new Dictionary<string, object> {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<MemberMilkProfileResponse, int, FiltersMeta>(
                    MemberQueries.GetMemberMilkProfileList,
                    DbConstants.Main, parameters, null);

            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);
            var filteredCount = filtered.Count();

            return new ListsResponse<MemberMilkProfileResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
