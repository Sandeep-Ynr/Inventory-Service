using MilkMatrix.Domain.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.State
{
    public class StateUpsertModel
    {
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? CountryId { get; set; }
        public string? AreaCode { get; set;}
        public bool? IsStatus { get; set; }

        //public GetActionType ActionType { get; set; } = GetActionType.All;
        //public bool? IsActive { get; set; }
        //public string? AreaCode { get; set; }


        //public int? CreatedBy { get; set; }
        //public int? ModifyBy { get; set; }
    }
}
