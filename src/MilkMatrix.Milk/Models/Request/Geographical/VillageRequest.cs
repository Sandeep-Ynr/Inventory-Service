using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class VillageRequest
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;
        public int? VillageId { get; set; }
        public int? TehsilId { get; set; }
        public bool? IsActive { get; set; }
        public string? TehsilName { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
