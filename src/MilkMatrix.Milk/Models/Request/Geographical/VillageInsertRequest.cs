using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class VillageInsertRequest
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;

        public int? VillageId { get; set; }
        public int? TehsilId { get; set; }
        public bool? IsActive { get; set; }
        //public int? Serial { get; set; }
        public string? VillageName { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        //public DateTime? CreatedOn { get; set; }
        //public int? ModifyBy { get; set; }
        //public DateTime? ModifyOn { get; set; }

    }
}
