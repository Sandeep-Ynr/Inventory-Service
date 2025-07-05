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
}
