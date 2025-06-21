namespace MilkMatrix.Core.Abstractions.Listings.Request;

public interface IListsRequest
{
    int Limit { get; set; }
    int Offset { get; set; }
    Dictionary<string, object>? Search { get; set; }
    Dictionary<string, object>? Sort { get; set; }
}
