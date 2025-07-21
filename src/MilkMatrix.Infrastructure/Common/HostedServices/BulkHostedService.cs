using Microsoft.Extensions.Hosting;
using MilkMatrix.Core.Abstractions.HostedServices;
using MilkMatrix.Core.Abstractions.Logger;

namespace MilkMatrix.Infrastructure.Common.HostedServices;

internal class BulkHostedService : BackgroundService, IBulkHostedService
{
    private readonly IBulkProcessingTasks taskQueue;
    private readonly ILogging logger;

    public BulkHostedService(IBulkProcessingTasks taskQueue, ILogging logger)
    {
        this.taskQueue = taskQueue;
        this.logger = logger.ForContext("ServiceName", nameof(BulkHostedService));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInfo("BulkHostedService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            Func<CancellationToken, Task> workItem = null;
            try
            {
                workItem = await taskQueue.DequeueBulkItemAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                logger.LogInfo("BulkHostedService is stopping due to cancellation.");
                break;
            }
            catch (Exception ex)
            {
                logger.LogError("Error while dequeuing work item.", ex);
                continue;
            }

            try
            {
                if (workItem != null)
                {
                    await workItem(stoppingToken);
                    logger.LogInfo("Background work item executed successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occurred executing background work item.", ex);
            }
        }

        logger.LogInfo("BulkHostedService has stopped.");
    }

    // Explicit interface implementation for control if needed
    async Task IBulkHostedService.StartAsync(CancellationToken cancellationToken)
        => await base.StartAsync(cancellationToken);

    async Task IBulkHostedService.StopAsync(CancellationToken cancellationToken)
        => await base.StopAsync(cancellationToken);
}
