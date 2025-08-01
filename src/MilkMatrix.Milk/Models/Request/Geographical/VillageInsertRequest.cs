using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class VillageInsertRequest
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;

        public string? BusinessId { get; set; }
        public string? VillageId { get; set; }
        public int? TehsilId { get; set; }
        public bool? IsActive { get; set; }
        public string? VillageName { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
    }
}
