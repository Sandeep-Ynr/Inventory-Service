using MilkMatrix.Core.Abstractions.Listings.Request;

namespace MilkMatrix.Core.Entities.Request;

public class ListsRequest : IListsRequest
{
    public int Limit { get; set; } = 0;
    public int Offset { get; set; } = 0;

    public string? Search { get; set; }
    public Dictionary<string, object>? Filters { get; set; }
    public Dictionary<string, object>? Sort { get; set; }
}
