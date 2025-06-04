using MilkMatrix.Logging.Contracts;
using Serilog;

namespace MilkMatrix.Logging.Implementation;


public class Logs : ILogs
{
    private readonly ILogger logger;

    public Logs(ILogger logger)
    {
        this.logger = logger;
    }

    public void LogInfo(string message) => logger.Information(message);
    public void LogWarning(string message) => logger.Warning(message);
    public void LogError(string message, Exception? ex = null)
    {
        if (ex != null)
            logger.Error(ex, message);
        else
            logger.Error(message);
    }

    public ILogs ForContext(string propertyName, object value) => new Logs(logger.ForContext(propertyName, value));
}
