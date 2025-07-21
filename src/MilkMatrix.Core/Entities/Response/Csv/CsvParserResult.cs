namespace MilkMatrix.Core.Entities.Response.Csv;

public class CsvParserResult<T>
{
    public List<T> Records { get; set; } = [];
    public List<CsvParserError> Errors { get; set; } = [];
}
