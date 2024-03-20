using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IConnet.Presale.WebApp.Models.Presales;
using Mapster;

namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : IndexPageBase
{
    [Inject] public IWorkPaperHttpClient WorkPaperHttpClient { get; set; } = default!;
    [Inject] public OptionService OptionService { get; set; } = default!;

    private ICollection<string> _rootCauses = default!;

    protected string SelectedRootCause = string.Empty;
    protected string RootCauseFilter { get; set; } = string.Empty;
    protected IEnumerable<string> RootCauses => GetFilteredRootCauses();
    protected int DropdownHeight => Math.Min(RootCauses.Count() * 40, 200);
    protected string DropdownHeightPx => $"{DropdownHeight}px";
    protected bool IsPopoverVisible { get; set; }
    protected bool FirstClick { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        FirstClick = true;
        IsPopoverVisible = true;

        _rootCauses = new List<string>(OptionService.RootCauseOptions);
    }

    protected void OpenPopover()
    {
        LogSwitch.Debug("before:{0}", IsPopoverVisible);
        // if (FirstClick)
        // {
        //     IsPopoverVisible = true;
        //     FirstClick = false;
        // }
        // else
        // {
        // }
        IsPopoverVisible = !IsPopoverVisible;
        LogSwitch.Debug("after:{0}", IsPopoverVisible);
    }

    protected async Task ForwardingAsync()
    {
        if (WorkPaper is null)
        {
            LogSwitch.Debug("Not found.");
            return;
        }

        bool isNotDone = WorkPaper.WorkPaperLevel < WorkPaperLevel.DoneProcessing;
        bool isInvalid = WorkPaper.WorkPaperLevel == WorkPaperLevel.ImportInvalid;

        if (isNotDone && !isInvalid)
        {
            LogSwitch.Debug("Not done yet.");
            return;
        }

        if (isInvalid)
        {
            LogSwitch.Debug("Forwarding invalid (REJECT)");
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

    // protected async Task OnSearchAsync(OptionsSearchEventArgs<string> eventArg)
    // {
    //     await Task.CompletedTask;

    //     var rootCauses = OptionService.RootCauseOptions;
    //     eventArg.Items = rootCauses.Where(option => option.StartsWith(eventArg.Text, StringComparison.OrdinalIgnoreCase));
    // }

    protected void OnRootCauseChanged(string rootCauses)
    {
        IsPopoverVisible = false;
        SelectedRootCause = rootCauses;
    }

    protected IEnumerable<string> GetFilteredRootCauses()
    {
        if (RootCauseFilter.IsNullOrWhiteSpace())
        {
            return _rootCauses;
        }
        else
        {
            return _rootCauses.Where(option => option.StartsWith(RootCauseFilter, StringComparison.OrdinalIgnoreCase));
        }
    }
}
