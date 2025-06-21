namespace MilkMatrix.Core.Abstractions.Listings.Response;

public interface IListsResponse<T>
{
    int Count { get; set; }
    IEnumerable<T> Results { get; set; }
    IEnumerable<IFiltersMeta> Filters { get; set; }
}
