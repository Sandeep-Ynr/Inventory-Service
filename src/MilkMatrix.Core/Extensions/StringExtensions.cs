namespace MilkMatrix.Core.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Format String message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="arg"></param>
    /// <returns>A formatted string</returns>
    public static string FormatString(this string message, string arg) => string.Format(message, arg);

    public static string GetImagePath(this string path, string domain) => string.IsNullOrEmpty(path)
            ? path
            : $"{domain.TrimEnd('/')}/{path?.TrimStart('/')}";
}
