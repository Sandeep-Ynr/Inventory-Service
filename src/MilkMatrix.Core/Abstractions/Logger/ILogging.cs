namespace MilkMatrix.Core.Abstractions.Logger;

public interface ILogging
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? ex = null);

    ILogging ForContext(string propertyName, object value);
}
