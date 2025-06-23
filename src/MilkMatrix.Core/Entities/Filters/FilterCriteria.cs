namespace MilkMatrix.Core.Entities.Filters;

public class FilterCriteria
{
    public string Property { get; set; } = string.Empty;
    public string Operator { get; set; } = "eq"; // eq, contains, gt, lt, etc.
    public object? Value { get; set; }
}
