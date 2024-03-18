using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Services;

public class SqlPushService
{
    private readonly IWorkPaperHttpClient _workPaperHttpClient;

    public SqlPushService(IWorkPaperHttpClient workPaperHttpClient)
    {
        _workPaperHttpClient = workPaperHttpClient;
    }

    public async Task SqlPushAsync(WorkPaper workPaper)
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
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var httpResult = await _workPaperHttpClient.InsertWorkPaperAsync(jsonModel);

        if (httpResult.IsSuccessStatusCode)
        {
            Log.Information("{0} has been successfully pushed to MySQL Database.", workPaper.ApprovalOpportunity.ApprovalOpportunityId);
        }
        else
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, options);
            var extension = problemDetails.GetProblemDetailsExtension();

            Log.Warning("Error {message}: ", extension.Errors.First().Message);
        }
    }
}
