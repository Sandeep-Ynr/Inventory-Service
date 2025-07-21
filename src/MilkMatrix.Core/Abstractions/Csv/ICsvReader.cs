using MilkMatrix.Core.Entities.Response.Csv;

namespace MilkMatrix.Core.Abstractions.Csv;

public interface ICsvReader
{
    Task<CsvParserResult<TDomain>> ReadCsvFile<TDomain>(byte[] file);
}
