namespace MilkMatrix.Core.Abstractions.HostedServices;

public interface IBulkHostedService
{
    /// <summary>
    /// Starts the bulk processing service.
    /// </summary>
    Task StartAsync(CancellationToken cancellationToken);
    /// <summary>
    /// Stops the bulk processing service.
    /// </summary>
    Task StopAsync(CancellationToken cancellationToken);
}
