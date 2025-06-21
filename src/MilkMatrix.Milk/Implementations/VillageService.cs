using System.Data;
using Microsoft.Extensions.Options;
using MilkMatrix.Core.Abstractions.Logger;
using MilkMatrix.Core.Abstractions.Repository.Factories;
using MilkMatrix.Domain.Entities.Responses;
using MilkMatrix.Infrastructure.Models.Config;
using MilkMatrix.Milk.Contracts.Geographical;
using MilkMatrix.Milk.Models.Request;
using MilkMatrix.Milk.Models.Response;
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
                {"DistrictId", request.DistrictId},
                {"StateId", request.StateId },
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
                {"DistrictId", request.DistrictId},
                {"StateId", request.StateId },
                {"IsStatus", request.IsActive}
            };

            var response = await repository.QueryAsync<VillageResponse>(VillageQueries.GetVillage, requestParams, null, CommandType.StoredProcedure);

            return response;
        }


    }
}
