using MilkMatrix.Core.Entities.Enums;

namespace MilkMatrix.Milk.Models.Request.Geographical;

public class StateRequest
{
    public ReadActionType ActionType { get; set; } = ReadActionType.All;

    public int? StateId { get; set; }

    public string? AreaCode { get; set; }

    public int? CountryId { get; set; }

    public bool? IsActive { get; set; }
    public string? StateName { get; set; }

    public int? CreatedBy { get; set; }
    public int? ModifyBy { get; set; }



}
