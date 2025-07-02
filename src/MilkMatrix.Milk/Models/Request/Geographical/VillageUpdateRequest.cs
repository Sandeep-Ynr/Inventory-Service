using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class VillageUpdateRequest
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;
        public int? VillageId { get; set; }
        public int? TehsilId { get; set; }
        public bool? IsActive { get; set; }
        public string? VillageName { get; set; }
        public int? ModifyBy { get; set; }
    }
}
