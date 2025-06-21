using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.Tehsil
{
    public class TehsilRequestModel
    {
        public GetActionType ActionType { get; set; } = GetActionType.All;
        public int TehsilId { get; set; }
        public string? TehsilName { get; set; }

        public int DistrictId { get; set; }
        public int StateId { get; set; }
        public bool? IsActive { get; set; }

        public bool IsStatus { get; set; }      
        public bool IsDeleted { get; set; }     

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public int? ModifyBy { get; set; }
        public DateTime? ModifyOn { get; set; }
    }
}
