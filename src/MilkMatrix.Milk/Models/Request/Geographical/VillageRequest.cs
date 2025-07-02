using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class VillageRequest
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;
        //public int? Serial { get; set; }
        public int? VillageId { get; set; }
        public int? TehsilId { get; set; }
        public bool? IsActive { get; set; }
        public string? VillageName { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
