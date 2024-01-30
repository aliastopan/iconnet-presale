using IConnet.Presale.WebApp.Components.Dialogs;

namespace IConnet.Presale.WebApp.Components.Pages;

public partial class CrmVerificationPage
{
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;
    [Inject] public BroadcastService BroadcastService { get; set; } = default!;

    private const int _itemPerPage = 10;
    private readonly PaginationState _pagination = new PaginationState { ItemsPerPage = _itemPerPage };

    private bool _isLoading = false;

    private IQueryable<WorkPaper>? _workPapers;

    protected override async Task OnInitializedAsync()
    {
        List<WorkPaper> workload = await WorkloadManager.FetchWorkloadAsync(CacheFetchMode.OnlyImportUnverified);
        _workPapers = workload.AsQueryable();

        BroadcastService.Subscribe(OnUpdateWorkloadAsync);
    }

    private async Task OnUpdateWorkloadAsync(string message)
    {
        List<WorkPaper> workload = await WorkloadManager.FetchWorkloadAsync(CacheFetchMode.OnlyImportUnverified);
        _workPapers = workload.AsQueryable();

        Log.Warning(message);

        // ensure component update is handle by UI thread
        await InvokeAsync(() =>
        {
            StateHasChanged();
            Log.Warning("Re-render 'CRM Verify Page'.");
        });
    }

    private async Task OnRowSelected(FluentDataGridRow<WorkPaper> row)
    {
        if (row.Item is not null)
        {
            await OpenDialogAsync(row.Item);
        }
    }

    private async Task OpenDialogAsync(WorkPaper workPaper)
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

    private async Task VerifyCrmAsync(WorkPaper workPaper)
    {
        _isLoading = true;

        await WorkloadManager.UpdateWorkloadAsync(workPaper);

        var message = $"CRM Import of '{workPaper.ApprovalOpportunity.IdPermohonan}' has been verified";
        await BroadcastService.BroadcastMessageAsync(message);

        _isLoading = false;
    }
}