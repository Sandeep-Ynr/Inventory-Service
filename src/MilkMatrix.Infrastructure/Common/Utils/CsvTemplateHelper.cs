using System.Reflection;
using System.Text;

namespace MilkMatrix.Infrastructure.Common.Utils;

public static class CsvTemplateHelper
{
    public static byte[] GenerateCsvTemplate<T>()
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var header = string.Join(",", properties.Select(p => p.Name));
        var sb = new StringBuilder();
        sb.AppendLine(header);
        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}
