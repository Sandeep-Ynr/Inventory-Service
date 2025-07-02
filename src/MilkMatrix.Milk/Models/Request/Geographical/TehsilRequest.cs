using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class TehsilRequest
    {
        public ReadActionType ActionType { get; set; } = ReadActionType.All;
        public int TehsilId { get; set; }
        public string? TehsilName { get; set; }
        public int? DistrictId { get; set; }
        public bool? IsActive { get; set; }
        public int CreatedBy { get; set; }

    }
}
