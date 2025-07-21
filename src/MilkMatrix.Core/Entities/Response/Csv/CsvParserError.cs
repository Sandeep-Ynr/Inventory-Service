namespace MilkMatrix.Core.Entities.Response.Csv;

public class CsvParserError
{
    public int RowNumber { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string? RawData { get; set; }
}
