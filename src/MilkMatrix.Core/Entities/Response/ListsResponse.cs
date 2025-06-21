using MilkMatrix.Core.Abstractions.Listings.Response;

namespace MilkMatrix.Core.Entities.Response
{
    public class ListsResponse<T> : IListsResponse<T>
    {
        public int Count { get; set; }
        public IEnumerable<T> Results { get; set; } = Enumerable.Empty<T>();
        public IEnumerable<IFiltersMeta> Filters { get; set; } = Enumerable.Empty<FiltersMeta>();
    }
}
