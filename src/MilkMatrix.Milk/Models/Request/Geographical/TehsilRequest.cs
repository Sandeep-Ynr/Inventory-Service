using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class TehsilRequest
    {
        public GetActionType ActionType { get; set; } = GetActionType.All;
        public int TehsilId { get; set; }
        public string? TehsilName { get; set; }

        public int DistrictId { get; set; }
        //public int StateId { get; set; }

        public bool IsStatus { get; set; }      // Usually used to indicate active/inactive
        public bool IsDeleted { get; set; }     // Soft delete flag

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public int? ModifyBy { get; set; }
        public DateTime? ModifyOn { get; set; }
          public bool? IsActive { get; set; }
    }
}
