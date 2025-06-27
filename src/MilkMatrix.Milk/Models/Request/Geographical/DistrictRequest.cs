using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class DistrictRequest
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;
        public int? DistrictId { get; set; }
        public int? StateId { get; set; }


        public string? DistrictName { get; set; }

        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifyBy { get; set; }
    }
}
