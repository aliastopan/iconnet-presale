using IConnet.Presale.WebApp.Components.Dialogs;

namespace IConnet.Presale.WebApp.Components.Pages;

public class HelpdeskStagingPageBase : WorkloadPageBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    private readonly string _pageName = "Helpdesk staging page";
    private readonly static int _stagingLimit = 5;

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

        var isNotStaged = workPaper!.HelpdeskInCharge.IsEmptySignature();
        var hasStageExpired = workPaper!.HelpdeskInCharge.IsDurationExceeded(now, duration);

        if (isNotStaged || hasStageExpired)
        {
            await OpenDialogAsync(row.Item);
        }
        else
        {
            await StagingResultNotificationAsync(workPaper!.HelpdeskInCharge.Alias);
        }
    }

    protected async Task StagingResultNotificationAsync(string inChargeAlias)
    {
        var intent = ToastIntent.Warning;
        var message = await SessionService.IsAliasMatch(inChargeAlias)
            ? "Presale telah Anda tampung."
            : $"Presale masih diproses oleh {inChargeAlias}.";

        ToastService.ShowToast(intent, message);
    }

    protected void StagingResultNotification()
    {
        var intent = ToastIntent.Error;
        var message = $"Jumlah tampungan Kertas Kerja ({_stagingLimit}) telah melebihi batas. ";
        ToastService.ShowToast(intent, message);
    }

    protected async Task OpenDialogAsync(WorkPaper workPaper)
    {
        var parameters = new DialogParameters()
        {
            Title = "Tampung Kertas Kerja",
            TrapFocus = true,
            Width = "500px",
        };

        var dialog = await DialogService.ShowDialogAsync<WorkloadStagingDialog>(workPaper, parameters);
        var result = await dialog.Result;

        if (result.Cancelled || result.Data == null)
        {
            return;
        }

        var dialogData = (WorkPaper)result.Data;
        await StageWorkloadAsync(dialogData);
    }

    private async Task StageWorkloadAsync(WorkPaper workPaper)
    {
        var count = await GetStageCountAsync();
        if (count > _stagingLimit)
        {
            workPaper.HelpdeskInCharge = RevertStagingSignature();
            StagingResultNotification();

            return;
        }

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"{workPaper.HelpdeskInCharge.Alias} has staged '{workPaper.ApprovalOpportunity.IdPermohonan}'";
        await BroadcastService.BroadcastMessageAsync(message);
    }

    private async Task<int> GetStageCountAsync()
    {
        var alias = await SessionService.GetSessionAliasAsync();
        var count = WorkPapers!.Where(x => x.HelpdeskInCharge.Alias == alias).Count();

        Log.Warning("Current staging count {0}", count);
        return count;
    }

    private ActionSignature RevertStagingSignature()
    {
        return new ActionSignature
        {
            AccountIdSignature = Guid.Empty,
            Alias = string.Empty,
            TglAksi = DateTimeService.Zero
        };
    }
}
