using MilkMatrix.Infrastructure.Common.Logger.Interface;
using MilkMatrix.Logging.Contracts;

namespace MilkMatrix.Infrastructure.Common.Logger.Implementation;

public class LoggingAdapter : ILogging
{
    private readonly ILogs logging;

    public LoggingAdapter(ILogs logging)
    {
        this.logging = logging;
    }

    public void LogInfo(string message) => logging.LogInfo(message);
    public void LogWarning(string message) => logging.LogWarning(message);
    public void LogError(string message, Exception? ex = null) => logging.LogError(message, ex);

    public ILogging ForContext(string propertyName, object value) => new LoggingAdapter(logging.ForContext(propertyName, value));

}
