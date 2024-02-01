using IConnet.Presale.WebApp.Components.Dialogs;

namespace IConnet.Presale.WebApp.Components.Pages;

public class HelpdeskStagingPageBase : WorkloadPageBase
{
    [Inject] public IDialogService DialogService { get; set; } = default!;

    private readonly string _pageName = "Helpdesk staging page";
    private readonly GridSort<WorkPaper> _sortByIdPermohonan = GridSort<WorkPaper>
        .ByAscending(workPaper => workPaper.ApprovalOpportunity.IdPermohonan);

    // TODO: move sort to `WorkloadPageBase`
    protected GridSort<WorkPaper> SortByIdPermohonan => _sortByIdPermohonan;

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

        var dialog = await DialogService.ShowDialogAsync<WorkloadClaimDialog>(workPaper, parameters);
        var result = await dialog.Result;

        if (!result.Cancelled && result.Data != null)
        {
            var dialogData = (WorkPaper)result.Data;
            await ClaimWorkloadAsync(dialogData);
        }
    }

    protected async Task ClaimWorkloadAsync(WorkPaper workPaper)
    {
        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"{workPaper.HelpdeskInCharge.Alias} has claimed '{workPaper.ApprovalOpportunity.IdPermohonan}'";
        await BroadcastService.BroadcastMessageAsync(message);
    }
}
