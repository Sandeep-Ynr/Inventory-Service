using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Api.Models.Request.Geographical.State;

public class StateRequestModel
{
    public int? StateId { get; set; }

    public ReadActionType ActionType { get; set; } = ReadActionType.All;
    public int? CountryId { get; set; }
    public bool? IsActive { get; set; }
    public string? AreaCode { get; set; }
    public string? StateName { get; set; }

    public int? CreatedBy { get; set; }
    public int? ModifyBy { get; set; }

}
