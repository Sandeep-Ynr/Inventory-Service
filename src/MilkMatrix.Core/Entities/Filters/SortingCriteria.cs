namespace MilkMatrix.Core.Entities.Filters;

public class SortingCriteria
{
    public string Property { get; set; } = string.Empty;
    public string Direction { get; set; } = "ASC"; // ASC or DESC
}
