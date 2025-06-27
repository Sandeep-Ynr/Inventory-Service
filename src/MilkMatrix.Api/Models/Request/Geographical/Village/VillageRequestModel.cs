using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Village
{
    public class VillageRequestModel
    {
        public GetActionType? ActionType { get; set; } = GetActionType.All;

        public int? VillageId { get; set; }

        public string? VillageName { get; set; }
        public int? TehsilId { get; set; }
        public int? DistrictId { get; set; }

        public int? StateId { get; set; }

        public bool? IsActive { get; set; }
    }
}
