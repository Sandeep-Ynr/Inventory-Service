using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical
{
    public class StateUpdateRequest
    {
        public int StateId { get; set; }
        public string? StateName { get; set; }
        public int? CountryId { get; set; }
        public string? AreaCode { get; set; }
        public CrudActionType ActionType { get; set; }
        public int ModifyBy { get; set; }
        public bool? IsActive { get; set; }
    }
}
