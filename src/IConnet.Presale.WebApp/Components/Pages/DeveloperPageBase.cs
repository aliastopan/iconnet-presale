using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Presales;
using Mapster;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : IndexPageBase
{
    [Inject] public IWorkPaperHttpClient WorkPaperHttpClient { get; set; } = default!;

    protected async Task ForwardingAsync()
    {
        if (WorkPaper is null)
        {
            LogSwitch.Debug("Not found.");
            return;
        }

        if (WorkPaper.WorkPaperLevel < WorkPaperLevel.DoneProcessing)
        {
            LogSwitch.Debug("Not done yet.");
            return;
        }

        var jsonModel = new WorkPaperFlatJsonModel(WorkPaper);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        LogSwitch.Debug("Forwarding...");

        var httpResult = await WorkPaperHttpClient.InsertWorkPaperAsync(jsonModel);

        if (httpResult.IsSuccessStatusCode)
        {
            LogSwitch.Debug("Success: {0}", httpResult.Content);
        }
        else
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpResult.Content, options);
            var extension = problemDetails.GetProblemDetailsExtension();

            LogSwitch.Debug("Error {message}: ", extension.Errors.First().Message);
        }
    }
}
