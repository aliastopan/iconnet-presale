namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class SqlSynchronizationManager : ISqlSynchronizationManager
{
    private readonly Queue<(string id, Task<bool> task)> _sqlSynchronizeTasks;

    public SqlSynchronizationManager(IWorkPaperHttpClient workPaperHttpClient)
    {
        _sqlSynchronizeTasks = new Queue<(string id, Task<bool> task)>();
    }

    public void EnqueueSynchronizeTask(string operationId, Task<bool> task)
    {
        lock (_sqlSynchronizeTasks)
        {
            _sqlSynchronizeTasks.Enqueue((operationId, task));
        }
    }

    public async Task ProcessSynchronizeTasks()
    {
        while (true)
        {
            (string id, Task<bool> task) sqlSynchronizeTasks;

            lock (_sqlSynchronizeTasks)
            {
                if (_sqlSynchronizeTasks.Count == 0)
                {
                    break;
                }

                sqlSynchronizeTasks = _sqlSynchronizeTasks.Dequeue();
            }

            try
            {
                bool result = await sqlSynchronizeTasks.task;

                if (result)
                {
                    LogSwitch.Debug("SQL Push has succeed.");
                }
                else
                {
                    LogSwitch.Debug("SQL Push has failed.");
                }
            }
            catch (Exception exception)
            {
                Log.Fatal("Error executing task for operation {0}: {1}", sqlSynchronizeTasks.id, exception.Message);
            }
        }
    }
}
