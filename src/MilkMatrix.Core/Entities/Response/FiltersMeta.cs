using MilkMatrix.Core.Abstractions.Listings.Response;

namespace MilkMatrix.Core.Entities.Response;

public class FiltersMeta : IFiltersMeta
{
    public string Key { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public IEnumerable<string>? ValuesAllowed { get; set; }
}
