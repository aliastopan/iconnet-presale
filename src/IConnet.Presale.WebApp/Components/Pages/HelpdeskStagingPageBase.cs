using IConnet.Presale.WebApp.Components.Dialogs;

namespace IConnet.Presale.WebApp.Components.Pages;

public class HelpdeskStagingPageBase : WorkloadPageBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    private readonly string _pageName = "Helpdesk staging page";

    public string GridTemplateCols
    {
        get => $"185px 150px 120px 150px 150px 150px 150px 150px 150px 150px 150px 150px 150px 250px 225px;";
    }

    protected override async Task OnInitializedAsync()
    {
        PageName = _pageName;
        CacheFetchMode = CacheFetchMode.OnlyImportVerified;

        await base.OnInitializedAsync();
    }

    protected async Task OnRowSelected(FluentDataGridRow<WorkPaper> row)
    {
        if (row.Item is null)
        {
            return;
        }

        var workPaper = row.Item;
        Log.Warning("Selected row {0}", workPaper is null ? "null" : workPaper.ApprovalOpportunity.IdPermohonan);

        var now = DateTimeService.DateTimeOffsetNow.DateTime;
        var duration = new TimeSpan(0, 5, 0);
        var timeRemaining = workPaper!.HelpdeskInCharge.GetDurationRemaining(now, duration);
        var label = timeRemaining > TimeSpan.Zero ? "Active" : "Expired";

        Log.Warning("Time remaining: {0} {1}", timeRemaining, label);

        var hasYetStaged = workPaper!.HelpdeskInCharge.IsEmptySignature();
        var isStageExpired = workPaper!.HelpdeskInCharge.IsDurationExceeded(now, duration);

        if (hasYetStaged || isStageExpired)
        {
            await OpenDialogAsync(row.Item);
        }
        else
        {
            await ToastNotificationAsync(workPaper!.HelpdeskInCharge.Alias);
        }
    }

    protected async Task ToastNotificationAsync(string inChargeAlias)
    {
        var intent = ToastIntent.Warning;
        var message = await SessionService.IsAliasMatch(inChargeAlias)
            ? "Presale telah Anda tampung."
            : $"Presale masih diproses oleh {inChargeAlias}.";

        ToastService.ShowToast(intent, message);
    }

    protected async Task OpenDialogAsync(WorkPaper workPaper)
    {
        var parameters = new DialogParameters()
        {
            Title = "Stage Workload",
            TrapFocus = true,
            Width = "500px",
        };

        var dialog = await DialogService.ShowDialogAsync<WorkloadStageDialog>(workPaper, parameters);
        var result = await dialog.Result;

        if (!result.Cancelled && result.Data != null)
        {
            var dialogData = (WorkPaper)result.Data;
            await StageWorkloadAsync(dialogData);
        }
    }

    protected async Task StageWorkloadAsync(WorkPaper workPaper)
    {
        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"{workPaper.HelpdeskInCharge.Alias} has claimed '{workPaper.ApprovalOpportunity.IdPermohonan}'";
        await BroadcastService.BroadcastMessageAsync(message);
    }
}
