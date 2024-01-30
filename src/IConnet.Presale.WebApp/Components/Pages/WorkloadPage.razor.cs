namespace IConnet.Presale.WebApp.Components.Pages;

public partial class WorkloadPage
{
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; set; } = default!;

    private const int _itemPerPage = 10;
    private readonly PaginationState _pagination = new PaginationState { ItemsPerPage = _itemPerPage };

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
            Log.Warning("Re-render 'Workload Page'.");
        });
    }
}