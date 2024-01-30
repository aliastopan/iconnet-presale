using IConnet.Presale.WebApp.Components.Dialogs;

namespace IConnet.Presale.WebApp.Components.Pages;

public partial class HelpdeskPage
{
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public BroadcastService BroadcastService { get; set; } = default!;

    private const int _itemPerPage = 10;
    private readonly PaginationState _pagination = new PaginationState { ItemsPerPage = _itemPerPage };
    private readonly GridSort<WorkPaper> _sortByIdPermohonan = GridSort<WorkPaper>
        .ByAscending(workPaper => workPaper.ApprovalOpportunity.IdPermohonan);

    private IQueryable<WorkPaper>? _workPapers;

    protected override async Task OnInitializedAsync()
    {
        List<WorkPaper> workload = await WorkloadManager.FetchWorkloadAsync(CacheFetchMode.OnlyImportVerified);
        _workPapers = workload.AsQueryable();

        BroadcastService.Subscribe(OnUpdateWorkloadAsync);
    }

    private async Task OnUpdateWorkloadAsync(string message)
    {
        List<WorkPaper> workload = await WorkloadManager.FetchWorkloadAsync(CacheFetchMode.OnlyImportVerified);
        _workPapers = workload.AsQueryable();

        Log.Warning(message);

        // ensure component update is handle by UI thread
        await InvokeAsync(() =>
        {
            StateHasChanged();
            Log.Warning("Re-render 'Helpdesk Page'.");
        });
    }

    private async Task OnRowSelected(FluentDataGridRow<WorkPaper> row)
    {
        if (row.Item is not null)
        {
            var workPaper = row.Item;
            Log.Warning("Selected row {0}", workPaper is null ? "null" : workPaper.ApprovalOpportunity.IdPermohonan);

            await OpenDialogAsync(row.Item);
        }
    }

    private async Task OpenDialogAsync(WorkPaper workPaper)
    {
        var parameters = new DialogParameters()
        {
            Title = "Klaim Workload",
            TrapFocus = true,
            Width = "500px",
        };

        var dialog = await DialogService.ShowDialogAsync<WorkloadStagingDialog>(workPaper, parameters);
        var result = await dialog.Result;

        if (!result.Cancelled && result.Data != null)
        {
            var dialogData = (WorkPaper)result.Data;
            await ClaimWorkloadAsync(dialogData);
        }
    }

    private async Task ClaimWorkloadAsync(WorkPaper workPaper)
    {
        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"{workPaper.HelpdeskInCharge.Alias} has claimed '{workPaper.ApprovalOpportunity.IdPermohonan}'";
        await BroadcastService.BroadcastMessageAsync(message);
    }
}
