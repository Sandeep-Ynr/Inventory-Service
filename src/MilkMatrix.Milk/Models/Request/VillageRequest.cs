using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request
{
    public class VillageRequest
    {
        public GetActionType ActionType { get; set; } = GetActionType.All;

        public int? VillageId { get; set; }
        public int? TehsilId { get; set; }
        public int? DistrictId { get; set; }
        public int? StateId { get; set; }
        public bool? IsActive { get; set; }
    }
}
