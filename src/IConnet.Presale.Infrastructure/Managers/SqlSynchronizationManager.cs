namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class SqlSynchronizationManager : ISqlSynchronizationManager
{
    private readonly Queue<Task<HttpResult>> _sqlPushTasks;

    public SqlSynchronizationManager()
    {
        _sqlPushTasks = new Queue<Task<HttpResult>>();
    }

    public void EnqueueSqlPushTask(Task<HttpResult> task)
    {
        lock (_sqlPushTasks)
        {
            _sqlPushTasks.Enqueue(task);
        }
    }

    public async Task ProcessSqlPushTasks()
    {
        while (true)
        {
            Task<HttpResult> sqlPushTasks;

            lock (_sqlPushTasks)
            {
                if (_sqlPushTasks.Count == 0)
                {
                    break;
                }

                sqlPushTasks = _sqlPushTasks.Dequeue();
            }

            try
            {
                HttpResult result = await sqlPushTasks;

                if (result.IsSuccessStatusCode)
                {
                    Log.Information("SQL Push has succeed.");
                }
                else
                {
                    Log.Warning("SQL Push has failed.");
                }
            }
            catch (Exception exception)
            {
                Log.Fatal("Error executing SQL Push task for operation {0}", exception.Message);
            }
        }
    }
}
