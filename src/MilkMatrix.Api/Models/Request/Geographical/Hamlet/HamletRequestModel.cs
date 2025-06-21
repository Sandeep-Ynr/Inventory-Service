using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Hamlet
{
    public class HamletRequestModel
    {
        public GetActionType? ActionType { get; set; } = GetActionType.All;

        public int? HamletId { get; set; }
        public int? VillageId { get; set; }
        public int? TehsilId { get; set; }
        public int? DistrictId { get; set; }

        public int? StateId { get; set; }

        public bool? IsActive { get; set; }
    }
}
