using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request
{
    public class HamletRequest
    {
        public GetActionType ActionType { get; set; } = GetActionType.All;
        public int ? HamletId { get; set;}
        public int? VillageId { get; set; }
        public int? TehsilId { get; set; }
        public int? DistrictId { get; set; }
        public int? StateId { get; set; }
        public bool? IsActive { get; set; }
    }
}
