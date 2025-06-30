using System.Data;
using Azure.Core;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Enums;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Models.Request;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;
using static MilkMatrix.Milk.Models.Queries.GeographicalQueries;

namespace MilkMatrix.Milk.Implementations
{
    public class VillageService : IVillageService
    {
        private readonly ILogging logging;

        private readonly AppConfig appConfig;

        private readonly IRepositoryFactory repositoryFactory;

        public VillageService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory)
        {
            this.logging = logging.ForContext("ServiceName", nameof(VillageService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(VillageService));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));

        }


        public async Task<IEnumerable<CommonLists>> GetSpecificLists(VillageRequest request)
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"VillageId", request.VillageId},
                {"TehsilId", request.TehsilId },
                //{"DistrictId", request.DistrictId},
                //{"StateId", request.StateId },
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<CommonLists>(VillageQueries.GetVillage, requestParams, null, CommandType.StoredProcedure);

            return response;

        }


        public async Task<IEnumerable<VillageResponse>> GetVillages(VillageRequest request)
        {
            var repository = repositoryFactory.Connect<VillageResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"VillageId", request.VillageId},
                {"TehsilId", request.TehsilId },
                //{"DistrictId", request.DistrictId},
                //{"StateId", request.StateId },
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<VillageResponse>(VillageQueries.GetVillage, requestParams, null, CommandType.StoredProcedure);

            return response;
        }

        public async Task<string> AddVillage(VillageRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", 1 },
                    { "VillageId", request.VillageId },
                    { "VillageName", request.VillageName ?? (object)DBNull.Value },
                    { "TehsilId", request.TehsilId },
                    { "IsStatus", request.IsStatus },
                    { "CreatedBy", request.CreatedBy },
                    { "ModifyBy", request.ModifyBy },
                };

                var response = await repository.QueryAsync<CommonLists>(
                    VillageQueries.AddVillage, requestParams, null, CommandType.StoredProcedure
                );

                // Return the inserted StateId or Name, etc. depending on your SP response
                return response?.FirstOrDefault()?.Name ?? "Insert failed or no response";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return "Error occurred";
            }
        }

        public async Task<string> UpdateVillage(VillageRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", 2 }, // 2 = Update
                    { "VillageId", request.VillageId },
                    { "VillageName", request.VillageName ?? (object)DBNull.Value },
                    { "TehsilId", request.TehsilId },
                    { "IsStatus", request.IsStatus },
                    { "CreatedBy", request.CreatedBy },
                    { "ModifyBy", request.ModifyBy }
                };

                var response = await repository.QueryAsync<CommonLists>(
                    VillageQueries.AddVillage, requestParams, null, CommandType.StoredProcedure
                );

                return response?.FirstOrDefault()?.Name ?? "Update failed or no response";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateVillage: " + ex.Message);
                return "Error occurred";
            }
        }

        public async Task<string> DeleteVillage(int villageId)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
                {
                    { "ActionType", 3 }, // 3 = Delete
                    { "VillageId", villageId }
                };
                var response = await repository.QueryAsync<CommonLists>(
                    VillageQueries.AddVillage, requestParams, null, CommandType.StoredProcedure
                );
                return response?.FirstOrDefault()?.Name ?? "Delete failed or no response";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DeleteVillage: " + ex.Message);
                return "Error occurred";
            }
        }



        public async Task<IEnumerable<VillageRequest>> GetByVillageId(int villageId)
        {
            var repository = repositoryFactory.Connect<VillageRequest>(DbConstants.Main);

            var requestParams = new Dictionary<string, object>
            {
                {"ActionType", 2}, // Use appropriate enum value
                {"VillageId", villageId}
            };

            var response = await repository.QueryAsync<VillageRequest>(
                VillageQueries.GetVillage,
                requestParams,
                null,
                CommandType.StoredProcedure
            );

            return response;
        }
    }
}
