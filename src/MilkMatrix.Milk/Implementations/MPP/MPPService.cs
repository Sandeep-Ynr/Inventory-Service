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
                    { "IsStatus", request.IsStatus ?? true },
                    { "CreatedBy", request.CreatedBy ?? 0 }
                };

                await repository.AddAsync(MPPQueries.AddMPP, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"MPP {request.MPPName} added successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in AddMPP: {request.MPPName}", ex);
                throw;
            }
        }

        public Task Delete(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IListsResponse<MPPResponse>> GetAll(IListsRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<MPPResponse?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MPPResponse>> GetMPP(MPPRequest request)
        {
            throw new NotImplementedException();
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
                    { "IsDeleted", request.IsDeleted ?? false },
                    { "ModifyBy", request.ModifiedBy ?? 0 }
                };

                await repository.UpdateAsync(MPPQueries.AddMPP, requestParams, CommandType.StoredProcedure);
                logging.LogInfo($"MPP {request.MPPName} updated successfully.");
            }
            catch (Exception ex)
            {
                logging.LogError($"Error in UpdateMPP: {request.MPPName}", ex);
                throw;
            }
        }
    }
}

        //public async Task<MPPMasterResponse?> GetById(int id)
        //{
        //    try
        //    {
        //        logging.LogInfo($"GetByIdAsync called for MPPMaster ID: {id}");
        //        var repo = repositoryFactory.ConnectDapper <
