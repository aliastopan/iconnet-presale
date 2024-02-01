using IConnet.Presale.WebApp.Components.Dialogs;

namespace IConnet.Presale.WebApp.Components.Pages;

public class CrmVerificationPageBase : WorkloadPageBase
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    private readonly string _pageName = "CRM Verification page";

    protected override async Task OnInitializedAsync()
    {
        PageName = _pageName;
        CacheFetchMode = CacheFetchMode.OnlyImportUnverified;

        await base.OnInitializedAsync();
    }

    protected async Task OnRowSelected(FluentDataGridRow<WorkPaper> row)
    {
        if (row.Item is not null)
        {
            await OpenDialogAsync(row.Item);
        }
    }

    protected async Task OpenDialogAsync(WorkPaper workPaper)
    {
        Log.Warning("Import status before: {0}", workPaper.ApprovalOpportunity.StatusImport);
        var parameters = new DialogParameters()
        {
            Title = "Verifikasi Import CRM",
            TrapFocus = true,
            Width = "500px",
        };

        var isImportVerified = workPaper.ApprovalOpportunity.StatusImport == ImportStatus.Verified;
        if (isImportVerified)
        {
            return;
        }

        var dialog = await DialogService.ShowDialogAsync<CrmVerificationDialog>(workPaper, parameters);
        var result = await dialog.Result;

        if (!result.Cancelled && result.Data != null)
        {
            var dialogData = (WorkPaper)result.Data;
            await VerifyCrmAsync(dialogData);

            Log.Warning("Import status after: {0}", dialogData.ApprovalOpportunity.StatusImport);
        }
    }

    protected async Task VerifyCrmAsync(WorkPaper workPaper)
    {
        IsLoading = true;

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"CRM Import of '{workPaper.ApprovalOpportunity.IdPermohonan}' has been verified";
        await BroadcastService.BroadcastMessageAsync(message);

        IsLoading = false;
    }
}