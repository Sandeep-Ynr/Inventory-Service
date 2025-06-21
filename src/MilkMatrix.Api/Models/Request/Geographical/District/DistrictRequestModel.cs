using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.District
{
    public class DistrictRequestModel
    {
        public GetActionType ActionType { get; set; } = GetActionType.All;
        public int? DistrictId { get; set; }
        public int? StateId { get; set; }
        public string? DistrictName { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifyBy { get; set; }
    }
}
