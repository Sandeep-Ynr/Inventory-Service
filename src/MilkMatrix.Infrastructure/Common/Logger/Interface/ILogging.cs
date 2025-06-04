using MilkMatrix.Logging.Contracts;

namespace MilkMatrix.Infrastructure.Common.Logger.Interface;

public interface ILogging
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? ex = null);

    ILogging ForContext(string propertyName, object value);
}
