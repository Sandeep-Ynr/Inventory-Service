using System.Data;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Core.Entities.Config;
using MilkMatrix.Core.Entities.Response;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Models.Request.Geographical;
using MilkMatrix.Milk.Models.Response.Geographical;
using static MilkMatrix.Milk.Models.Queries.GeographicalQueries;

namespace MilkMatrix.Milk.Implementations
{
    public class TehsilService : ITehsilService
    {
        private readonly ILogging logging;
        private readonly AppConfig appConfig;
        private readonly IRepositoryFactory repositoryFactory;


        public TehsilService(ILogging logging, IOptions<AppConfig> appConfig, IRepositoryFactory repositoryFactory)
        {
            this.logging = logging.ForContext("ServiceName", nameof(TehsilService));
            this.appConfig = appConfig.Value ?? throw new ArgumentNullException(nameof(appConfig));
            this.repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));

        }

        public async Task<IEnumerable<CommonLists>> GetSpecificLists(TehsilRequest request)
        {
            var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"TehsilId", request.TehsilId},
                {"DistrictId", request.DistrictId},
                //{"StateId", request.StateId },
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<CommonLists>(DistrictQueries.GetDistrict, requestParams, null, CommandType.StoredProcedure);

            return response;

        }

        public async Task<IEnumerable<TehsilResponse>> GetTehsils(TehsilRequest request)
        {
            var repository = repositoryFactory.Connect<TehsilResponse>(DbConstants.Main);
            var requestParams = new Dictionary<string, object>
            {
                {"ActionType",(int)request.ActionType },
                {"TehsilId", request.TehsilId},
                {"DistrictId", request.DistrictId},
                //{"StateId", request.StateId },
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<TehsilResponse>(TehsilQueries.GetTehsil, requestParams, null, CommandType.StoredProcedure);

            return response;
        }


        public async Task<string> AddTehsil(TehsilRequest request)
        {
            try
            {
                var repository = repositoryFactory.Connect<CommonLists>(DbConstants.Main);
                var requestParams = new Dictionary<string, object>
            {
                { "ActionType", 1 }, // 1 for insert
                { "TehsilId", request.TehsilId },
                { "TehsilName", request.TehsilName },
                { "DistrictId", request.DistrictId },
                //{ "StateId", request.StateId  },
                { "IsStatus", request.IsStatus  },
                { "IsDeleted", request.IsStatus  },
                { "CreatedBy", request.CreatedBy },
                { "ModifyBy", request.ModifyBy ?? (object)DBNull.Value },
            };

                var response = await repository.QueryAsync<CommonLists>(
                    TehsilQueries.AddTehsil, requestParams, null, CommandType.StoredProcedure
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


    }
}
