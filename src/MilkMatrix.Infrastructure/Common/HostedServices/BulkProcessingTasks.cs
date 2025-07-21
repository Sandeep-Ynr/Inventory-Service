using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using MilkMatrix.Core.Abstractions.HostedServices;
using MilkMatrix.Core.Abstractions.Logger;

namespace MilkMatrix.Infrastructure.Common.HostedServices
{
    internal class BulkProcessingTasks : IBulkProcessingTasks
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> items = new();
        private readonly SemaphoreSlim tasks = new(0);
        private readonly ILogging logger;

        public BulkProcessingTasks(ILogging logger)
        {
            this.logger = logger.ForContext("ServiceName", nameof(BulkProcessingTasks));
        }

        public void QueueBulkWorkItem(Func<CancellationToken, Task> workItem)
        {
            try
            {
                if (workItem == null) throw new ArgumentNullException(nameof(workItem));
                items.Enqueue(workItem);
                tasks.Release();
                logger.LogInfo("Background work item queued.");
            }
            catch (Exception ex)
            {
                logger.LogError("Error while queuing background work item.", ex);
                throw;
            }
        }

        public async Task<Func<CancellationToken, Task>> DequeueBulkItemAsync(CancellationToken cancellationToken)
        {
            try
            {
                await tasks.WaitAsync(cancellationToken);
                items.TryDequeue(out var workItem);
                logger.LogInfo("Background work item dequeued.");
                return workItem;
            }
            catch (Exception ex)
            {
                logger.LogError("Error while dequeuing background work item.", ex);
                throw;
            }
        }
    }
}
