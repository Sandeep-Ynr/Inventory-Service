using MilkMatrix.Core.Attributes;

namespace MilkMatrix.Core.Entities.Response;

public class CommonLists
{
    public int Id { get; set; }

    [GlobalSearch]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
