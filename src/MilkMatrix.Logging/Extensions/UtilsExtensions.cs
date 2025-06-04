namespace MilkMatrix.Logging.Extensions;

internal static class UtilsExtensions
{
  public static bool IsNullOrWhiteSpace(this String value) => string.IsNullOrWhiteSpace(value);

  public static bool ContainsAny(this string input, IEnumerable<string> keywords) =>
      keywords.Any(keyword => input.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) >= 0);

  public static string GetConcatenatedString(string hostName, string userKey, int? businessId)
  {

    var tokenKey = $"{hostName.ToLower()}|{userKey.ToLower()}|{DateTime.Now.ToString("yyyyMMdd")}";
    if (businessId != null)
    {
      tokenKey += $"|{businessId}";
    }
    return tokenKey + "`" + Guid.NewGuid().ToString();
  }

  public static string FormatString(this string message, string arg) => string.Format(message, arg);
}
