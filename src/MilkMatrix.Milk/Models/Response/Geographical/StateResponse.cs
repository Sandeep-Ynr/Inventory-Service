using MilkMatrix.Domain.Entities.Responses;

namespace MilkMatrix.Milk.Models.Response.Geographical;

public class StateResponse : CommonLists
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = string.Empty;

    public string GstStateCode { get; set; } = string.Empty;
}
