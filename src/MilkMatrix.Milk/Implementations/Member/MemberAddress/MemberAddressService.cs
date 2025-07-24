using MilkMatrix.Core.Abstractions.Listings.Request;
using MilkMatrix.Core.Abstractions.Listings.Response;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Milk.Contracts.Member.MemberAddress;
using MilkMatrix.Milk.Models.Request.Member.MemberAddress;
using MilkMatrix.Milk.Models.Response.Member.MemberAddress;
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

namespace MilkMatrix.Milk.Implementations.Member.Address
{
    public class MemberAddressService : IMemberAddressService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;

        public MemberAddressService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(MemberAddressService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }

        public async Task AddMemberAddress(MemberAddressInsertRequest request)
        {
             try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main); // Assuming CommonLists is appropriate
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Create},
                    {"MemberID", request.MemberID},
                    {"StateID", request.StateID},
                    {"DistrictID", request.DistrictID},
                    {"TehsilID", request.TehsilID},
                    {"VillageID", request.VillageID},
                    {"HamletID", request.HamletID ?? (object)DBNull.Value},
                    {"FullAddress", request.FullAddress},
                    {"Pincode", request.Pincode},
                     { "IsStatus", request.IsStatus ?? (object)DBNull.Value},
                    {"created_on", request.CreatedOn ?? (object)DBNull.Value},
                    {"created_by", request.CreatedBy ?? (object)DBNull.Value},
                    { "IsDeleted", request.IsDeleted ?? (object)DBNull.Value}
                };

            var message = await repository.AddAsync(MemberQueries.AddOrUpdateMemberAddress, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Member Address for Member ID {request.MemberID} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError("Error in AddMemberAddress", ex);
                throw;
            }
        }

        public async Task UpdateMemberAddress(MemberAddressUpdateRequest request)
        {
             try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main); // Assuming CommonLists is appropriate
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Update},
                    {"AddressID", request.AddressID},
                    {"MemberID", request.MemberID},
                    {"StateID", request.StateID},
                    {"DistrictID", request.DistrictID},
                    {"TehsilID", request.TehsilID},
                    {"VillageID", request.VillageID},
                    {"HamletID", request.HamletID ?? (object)DBNull.Value},
                    {"FullAddress", request.FullAddress},
                    {"Pincode", request.Pincode},
                    {"IsStatus", request.IsStatus ?? (object)DBNull.Value},
                    {"ModifyOn", request.ModifiedOn ?? (object)DBNull.Value},
                    {"ModifyBy", request.ModifiedBy ?? (object)DBNull.Value},
                    {"IsDeleted", request.IsDeleted ?? (object)DBNull.Value}
                };



                var message = await repository.UpdateAsync(MemberQueries.AddOrUpdateMemberAddress, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Member Address with ID {request.AddressID} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError("Error in UpdateMemberAddress", ex);
                throw;
            }
        }

        public async Task DeleteMemberAddress(long addressId, long userId)
        {
             try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main); // Assuming CommonLists is appropriate
                var requestParams = new Dictionary<string, object>
                {
                    {"ActionType", (int)CrudActionType.Delete},
                    {"AddressID", addressId},
                    {"modify_by", userId}
                };

                var response = await repository.DeleteAsync(MemberQueries.AddOrUpdateMemberAddress, requestParams, CommandType.StoredProcedure);

                logging.LogInfo($"Member Address with ID {addressId} deleted successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteMemberAddress for Address ID: {addressId}", ex);
                throw;
            }
        }

        public async Task<MemberAddressResponse?> GetMemberAddressById(long id)
        {
             try
            {
                logging.LogInfo($"GetById called for MemberAddress ID: {id}");
                var repo = repositoryFactory.ConnectDapper<MemberAddressResponse>(DbConstants.Main); // Assuming Dapper and MemberAddressResponse are appropriate
                var data = await repo.QueryAsync<MemberAddressResponse>(MemberQueries.GetMemberAddressList, new Dictionary<string, object>
                {
                    {"ActionType", (int)ReadActionType.Individual},
                    {"AddressID", id}
                }, null);

                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetMemberAddressById", ex);
                throw;
            }
        }

        public async Task<IEnumerable<MemberAddressResponse>> GetMemberAddresses(MemberAddressRequestModel request)
        {
             try
            {
                var repo = repositoryFactory.ConnectDapper<MemberAddressResponse>(DbConstants.Main); // Assuming Dapper and MemberAddressResponse are appropriate
                var parameters = new Dictionary<string, object>
                {
                    {"ActionType", (int)(request.ActionType ?? ReadActionType.All)},
                    {"AddressID", request.AddressID ?? (object)DBNull.Value},
                    {"MemberID", request.MemberID ?? (object)DBNull.Value},
                    {"StateID", request.StateID ?? (object)DBNull.Value},
                    {"DistrictID", request.DistrictID ?? (object)DBNull.Value},
                    {"TehsilID", request.TehsilID ?? (object)DBNull.Value},
                    {"VillageID", request.VillageID ?? (object)DBNull.Value},
                    {"is_status", request.is_status ?? (object)DBNull.Value},
                    {"is_deleted", request.is_deleted ?? (object)DBNull.Value}
                };

                return await repo.QueryAsync<MemberAddressResponse>(MemberQueries.GetMemberAddressList, parameters, null);
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetMemberAddresses", ex);
                throw;
            }
        }

        public async Task<IListsResponse<MemberAddressResponse>> GetAllMemberAddresses(IListsRequest request)
        {
             var parameters = new Dictionary<string, object> {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<MemberAddressResponse, int, FiltersMeta>(MemberQueries.GetMemberAddressList,
                    DbConstants.Main, parameters, null);
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);
            var filteredCount = filtered.Count();

            return new ListsResponse<MemberAddressResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
