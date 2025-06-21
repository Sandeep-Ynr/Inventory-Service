namespace MilkMatrix.Core.Abstractions.Listings.Response;

public interface IFiltersMeta
{
    string Key { get; set; }
    string Type { get; set; }
    string Label { get; set; }
    IEnumerable<string>? ValuesAllowed { get; set; }
}
