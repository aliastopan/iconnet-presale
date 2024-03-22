namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IWorkloadSynchronizationManager
{
    void EnqueueSynchronizeTask(string operationId, Task task);
    Task ProcessSynchronizeTasks();

    Task<int> PullRedisToInMemoryAsync();
}
