namespace MilkMatrix.Logging.Contracts;

public interface ILogs
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? ex = null);

    ILogs ForContext(string propertyName, object value);
}
