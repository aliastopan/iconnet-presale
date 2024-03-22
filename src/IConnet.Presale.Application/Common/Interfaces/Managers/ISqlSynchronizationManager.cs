namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface ISqlSynchronizationManager
{
    void EnqueueSynchronizeTask(string operationId, Task<bool> task);
    Task ProcessSynchronizeTasks();
}
