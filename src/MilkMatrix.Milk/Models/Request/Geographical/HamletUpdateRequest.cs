using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class HamletUpdateRequest
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;
        public int ? HamletId { get; set;}

        public string? HamletName { get; set; }
        public int? VillageId { get; set; }
        //public int? TehsilId { get; set; }
        //public int? DistrictId { get; set; }
        //public int? StateId { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifyBy { get; set; }

        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
