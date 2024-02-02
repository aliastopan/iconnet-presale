using IConnet.Presale.WebApp.Components.Dialogs;

namespace IConnet.Presale.WebApp.Components.Pages;

public class HelpdeskStagingPageBase : WorkloadPageBase
{
    [Inject] public IDialogService DialogService { get; set; } = default!;

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
        if (row.Item is not null)
        {
            var workPaper = row.Item;
            Log.Warning("Selected row {0}", workPaper is null ? "null" : workPaper.ApprovalOpportunity.IdPermohonan);

            await OpenDialogAsync(row.Item);
        }
    }

    protected async Task OpenDialogAsync(WorkPaper workPaper)
    {
        var parameters = new DialogParameters()
        {
            Title = "Klaim Workload",
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
