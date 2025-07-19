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
using MilkMatrix.Milk.Contracts.Logistics.Transporter;
using MilkMatrix.Milk.Models.Queries;
using MilkMatrix.Milk.Models.Request.Logistics.Transporter;
using MilkMatrix.Milk.Models.Response.Logistics.Transporter;

namespace MilkMatrix.Milk.Implementations
{
    public class TransporterService : ITransporterService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IQueryMultipleData queryMultipleData;
        public TransporterService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory, IQueryMultipleData queryMultipleData)
        {
            this.logging = logging.ForContext("ServiceName", nameof(TransporterService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
            this.queryMultipleData = queryMultipleData;
        }
        public async Task AddTransporter(TransporterInsertRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);

                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Create },
                    { "TransporterName", request.TransporterName },
                    { "LocalName", request.LocalName?? (object)DBNull.Value  },
                    { "Address", request.Address?? (object)DBNull.Value  },
                    { "PhoneNo", request.PhoneNo?? (object)DBNull.Value  },
                    { "MobileNo", request.MobileNo?? (object)DBNull.Value  },
                    { "Email", request.Email?? (object)DBNull.Value  },
                    { "Pincode", request.Pincode?? (object)DBNull.Value  },
                    { "RegistrationNo", request.RegistrationNo?? (object)DBNull.Value  },
                    { "ContactPerson", request.ContactPerson },
                    { "LocalContactPerson", request.LocalContactPerson?? (object)DBNull.Value  },
                    { "BankID", request.BankID },
                    { "BranchID", request.BranchID },
                    { "BranchCode", request.BranchCode?? (object)DBNull.Value  },
                    { "BankAccountNo", request.BankAccountNo?? (object)DBNull.Value  },
                    { "IFSC", request.IFSC ?? (object)DBNull.Value },
                    { "GSTIN", request.GSTIN ?? (object)DBNull.Value },
                    { "TdsPer", request.TdsPer?? (object)DBNull.Value  },
                    { "PanNo", request.PanNo?? (object)DBNull.Value  },
                    { "BeneficiaryName", request.BeneficiaryName ?? (object)DBNull.Value },
                    { "AgreementNo", request.AgreementNo?? (object)DBNull.Value  },
                    { "Declaration", request.Declaration ?? (object)DBNull.Value },
                    { "SecurityChequeNo", request.SecurityChequeNo?? (object)DBNull.Value  },
                    { "CompanyCode", request.CompanyCode },
                    { "StateID", request.StateID },
                    { "DistrictID", request.DistrictID },
                    { "TehsilID", request.TehsilID },
                    { "VillageID", request.VillageID },
                    { "HamletID", request.HamletID },
                    { "VendorID", request.VendorID?? (object)DBNull.Value },
                    { "SecurityAmount", request.SecurityAmount ?? (object)DBNull.Value },
                    { "IsStatus", request.IsStatus ?? (object)DBNull.Value },
                    { "CreatedBy", request.CreatedBy ?? (object)DBNull.Value }
                };
                var message = await repository.AddAsync(TransporterQueries.AddTransporter, requestParams, CommandType.StoredProcedure);
                if (message.StartsWith("Error"))
                {
                    throw new Exception($"Stored Procedure Error: {message}");
                }

                logging.LogInfo($"Transporter {request.TransporterName} added successfully with response: {message}");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddTransporter: {request.TransporterName}", ex);
                throw;
            }
        }
        public async Task UpdateTransporter(TransporterUpdateRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", (int)CrudActionType.Update },
                    { "TransporterID", request.TransporterID },
                    { "TransporterName", request.TransporterName },
                    { "LocalName", request.LocalName ?? (object)DBNull.Value },
                    { "Address", request.Address ?? (object)DBNull.Value },
                    { "PhoneNo", request.PhoneNo ?? (object)DBNull.Value },
                    { "MobileNo", request.MobileNo ?? (object)DBNull.Value },
                    { "Email", request.Email ?? (object)DBNull.Value },
                    { "Pincode", request.Pincode ?? (object)DBNull.Value },
                    { "RegistrationNo", request.RegistrationNo ?? (object)DBNull.Value },
                    { "ContactPerson", request.ContactPerson ?? (object)DBNull.Value },
                    { "LocalContactPerson", request.LocalContactPerson ?? (object)DBNull.Value },
                    { "BankID", request.BankID },
                    { "BranchID", request.BranchID },
                    { "BranchCode", request.BranchCode ?? (object)DBNull.Value },
                    { "BankAccountNo", request.BankAccountNo ?? (object)DBNull.Value },
                    { "IFSC", request.IFSC ?? (object)DBNull.Value },
                    { "GSTIN", request.GSTIN ?? (object)DBNull.Value },
                    { "TdsPer", request.TdsPer ?? (object)DBNull.Value },
                    { "PanNo", request.PanNo ?? (object)DBNull.Value },
                    { "BeneficiaryName", request.BeneficiaryName ?? (object)DBNull.Value },
                    { "AgreementNo", request.AgreementNo ?? (object)DBNull.Value },
                    { "Declaration", request.Declaration ?? (object)DBNull.Value },
                    { "SecurityChequeNo", request.SecurityChequeNo ?? (object)DBNull.Value },
                    { "CompanyCode", request.CompanyCode },
                    { "StateID", request.StateID },
                    { "DistrictID", request.DistrictID },
                    { "TehsilID", request.TehsilID },
                    { "VillageID", request.VillageID },
                    { "HamletID", request.HamletID },
                    { "SecurityAmount", request.SecurityAmount ?? (object)DBNull.Value },
                    { "VendorID", request.VendorID  },
                    { "IsStatus", request.IsStatus},
                    { "ModifiedBy", request.ModifiedBy ?? (object)DBNull.Value }
                };
                var message = await repository.UpdateAsync(TransporterQueries.AddTransporter, requestParams, CommandType.StoredProcedure);

                if (message.StartsWith("Error"))
                    throw new Exception($"Stored Procedure Error: {message}");

                logging.LogInfo($"Transporter {request.TransporterName} updated successfully with response: {message}");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateTransporter: {request.TransporterName}", ex);
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
                    {"TransporterID", id },
                    { "ModifiedBy", userId },
                };

                var response = await repository.DeleteAsync(
                   TransporterQueries.AddTransporter, requestParams, CommandType.StoredProcedure
                );

                logging.LogInfo($"Transporter Type with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                logging.LogError($"Error in DeleteAsync for Transporter id: {id}", ex);
                throw;
            }
        }
        public async Task<TransporterResponse?> GetById(int id)
        {
            try
            {
                logging.LogInfo($"GetById called for Transporter ID: {id}");
                var repo = repositoryFactory.ConnectDapper<TransporterResponse>(DbConstants.Main);
                var data = await repo.QueryAsync<TransporterResponse>("usp_transporter_list", new Dictionary<string, object>
                {
                    { "ActionType", (int)ReadActionType.Individual },
                    { "TransporterID", id }
                }, null);

                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in GetById for Transporter ID: {id}", ex);
                throw;
            }
        }
        public async Task<IEnumerable<TransporterResponse>> GetTransporters(TransporterRequest request)
        {
            try
            {
                var repo = repositoryFactory.ConnectDapper<TransporterResponse>(DbConstants.Main);
                var parameters = new Dictionary<string, object>
                {
                    { "ActionType", (int)(request.ActionType ?? ReadActionType.All) },
                    { "TransporterID", request.TransporterID ?? (object)DBNull.Value },
                    { "IsActive", request.IsStatus }
                };

                return await repo.QueryAsync<TransporterResponse>("usp_transporter_list", parameters, null);
            }
            catch (Exception ex)
            {
                logging.LogError("Error in GetTransporters", ex);
                throw;
            }
        }
        public async Task<IListsResponse<TransporterResponse>> GetAll(IListsRequest request)
        {
            var parameters = new Dictionary<string, object> {
                { "ActionType", (int)ReadActionType.All }
            };

            var (allResults, countResult, filterMetas) = await queryMultipleData
                .GetMultiDetailsAsync<TransporterResponse, int, FiltersMeta>(TransporterQueries.GetTransporterList,
                    DbConstants.Main, parameters, null);
            var filters = filterMetas.BuildFilterCriteriaFromRequest(request.Filters, request.Search);
            var sorts = filterMetas.BuildSortCriteriaFromRequest(request.Sort);
            var paging = new PagingCriteria { Offset = request.Offset, Limit = request.Limit };

            var filtered = allResults.AsQueryable().ApplyFilters(filters);
            var sorted = filtered.ApplySorting(sorts);
            var paged = sorted.ApplyPaging(paging);
            var filteredCount = filtered.Count();

            return new ListsResponse<TransporterResponse>
            {
                Count = filteredCount,
                Results = paged.ToList(),
                Filters = filterMetas
            };
        }
    }
}
