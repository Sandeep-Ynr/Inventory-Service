using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using MilkMatrix.Core.Abstractions.Csv;
using MilkMatrix.Core.Entities.Response.Csv;

namespace MilkMatrix.Infrastructure.Common.Csv;

public class CsvFileReader : ICsvReader
{
    public async Task<CsvParserResult<TDomain>> ReadCsvFile<TDomain>(byte[] file)
    {
        var result = new CsvParserResult<TDomain>();
        using var stream = new MemoryStream(file);
        using var reader = new StreamReader(stream);

        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            HeaderValidated = null,
            MissingFieldFound = null,
            ShouldSkipRecord = args => args.Row.Parser.Record?.All(field => string.IsNullOrWhiteSpace(field)) ?? false,
            PrepareHeaderForMatch = args => args.Header.ToLowerInvariant() // <-- Case-insensitive mapping
        };

        using var csv = new CsvReader(reader, configuration);

        int rowNumber = 1; // Header row
        try
        {
            await csv.ReadAsync();
            csv.ReadHeader();

            // Lowercase header check
            var csvHeaders = csv.HeaderRecord?.Select(h => h.ToLowerInvariant()).ToArray() ?? [];
            var modelProps = typeof(TDomain).GetProperties()
                .Select(p => p.Name.ToLowerInvariant()).ToArray();

            //var missing = modelProps.Except(csvHeaders).ToList();
            //if (missing.Any())
            //    throw new Exception($"Missing required headers: {string.Join(", ", missing)}");
        }
        catch (Exception ex)
        {
            result.Errors.Add(new CsvParserError
            {
                RowNumber = rowNumber,
                ErrorMessage = $"Header validation failed: {ex.Message}"
            });
            return result;
        }

        while (await csv.ReadAsync())
        {
            rowNumber++;
            try
            {
                var record = csv.GetRecord<TDomain>();
                result.Records.Add(record);
            }
            catch (Exception ex)
            {
                result.Errors.Add(new CsvParserError
                {
                    RowNumber = rowNumber,
                    ErrorMessage = ex.Message,
                    RawData = string.Join(",", csv.Parser.Record ?? [])
                });
            }
        }

        return result;
    }
}
