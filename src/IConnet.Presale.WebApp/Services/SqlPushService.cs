using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Services;

public class SqlPushService
{
    private readonly IWorkPaperHttpClient _workPaperHttpClient;
    private readonly ISqlSynchronizationManager _sqlSynchronizationManager;

    public SqlPushService(IWorkPaperHttpClient workPaperHttpClient,
        ISqlSynchronizationManager sqlSynchronizationManager)
    {
        _workPaperHttpClient = workPaperHttpClient;
        _sqlSynchronizationManager = sqlSynchronizationManager;
    }

    public void SqlPush(WorkPaper workPaper)
    {
        if (workPaper is null)
        {
            Log.Information("Not found.");
            return;
        }

        bool isNotDone = workPaper.WorkPaperLevel < WorkPaperLevel.DoneProcessing;
        bool isInvalid = workPaper.WorkPaperLevel == WorkPaperLevel.ImportInvalid;

        if (isNotDone && !isInvalid)
        {
            Log.Information("Not done yet.");
            return;
        }

        if (isInvalid)
        {
            Log.Information("Push invalid (REJECT)");
        }

        var jsonModel = new WorkPaperFlatJsonModel(workPaper);
        var httpResultTask = _workPaperHttpClient.InsertWorkPaperAsync(jsonModel);

        _sqlSynchronizationManager.EnqueueSqlPushTask(httpResultTask);
    }
}
