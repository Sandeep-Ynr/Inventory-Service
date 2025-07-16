namespace MilkMatrix.Core.Abstractions.Listings.Request;

public interface IListsRequest
{
    int Limit { get; set; }
    int Offset { get; set; }

    string? Search { get; set; }
    Dictionary<string, object>? Filters { get; set; }
    Dictionary<string, object>? Sort { get; set; }
}
