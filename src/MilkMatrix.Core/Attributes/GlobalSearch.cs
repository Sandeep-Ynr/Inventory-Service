namespace MilkMatrix.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class GlobalSearch : Attribute
{
    public string? Name { get; }
    public GlobalSearch() { }
    public GlobalSearch(string name) => Name = name;
}
