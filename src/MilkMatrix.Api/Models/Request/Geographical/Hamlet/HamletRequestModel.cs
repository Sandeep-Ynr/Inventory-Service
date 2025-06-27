using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Hamlet
{
    public class HamletRequestModel
    {
        public GetActionType? ActionType { get; set; } = GetActionType.All;
        public string HamletName { get; set; }
        public int? HamletId { get; set; }
        public int? VillageId { get; set; }
        public int? TehsilId { get; set; }
        public int? DistrictId { get; set; }

        public int? StateId { get; set; }
        public bool? IsStatus { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifyBy { get; set; }
    }
}
