namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IWorkloadForwardingManager
{
    void EnqueueForwardingTask(string operationId, Task task);
    Task ProcessForwardingTasks();
}
