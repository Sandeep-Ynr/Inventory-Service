namespace MilkMatrix.Core.Abstractions.HostedServices;

public interface IBulkProcessingTasks
{
    void QueueBulkWorkItem(Func<CancellationToken, Task> workItem);
    Task<Func<CancellationToken, Task>> DequeueBulkItemAsync(CancellationToken cancellationToken);
}
