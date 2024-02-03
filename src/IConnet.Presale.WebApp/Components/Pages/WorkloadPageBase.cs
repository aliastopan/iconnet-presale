namespace IConnet.Presale.WebApp.Components.Pages;

public class WorkloadPageBase : ComponentBase
{
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; init; } = default!;

    private const int _itemPerPage = 10;
    private readonly PaginationState _pagination = new PaginationState { ItemsPerPage = _itemPerPage };

    private IQueryable<WorkPaper>? _workPapers;
    private readonly GridSort<WorkPaper> _sortByIdPermohonan = GridSort<WorkPaper>
        .ByAscending(workPaper => workPaper.ApprovalOpportunity.IdPermohonan);

    protected PaginationState Pagination => _pagination;
    protected CacheFetchMode CacheFetchMode { get; set; } = CacheFetchMode.OnlyImportVerified;
    protected virtual IQueryable<WorkPaper>? WorkPapers => _workPapers;
    protected GridSort<WorkPaper> SortByIdPermohonan => _sortByIdPermohonan;

    protected string PageName { get; set; } = "Workload page (base)";
    protected virtual bool IsLoading { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        List<WorkPaper> workload = await WorkloadManager.FetchWorkloadAsync(CacheFetchMode);
        _workPapers = workload.AsQueryable();

        BroadcastService.Subscribe(OnUpdateWorkloadAsync);
    }

    private async Task OnUpdateWorkloadAsync(string message)
    {
        List<WorkPaper> workload = await WorkloadManager.FetchWorkloadAsync(CacheFetchMode);
        _workPapers = workload.AsQueryable();

        Log.Warning(message);

        // ensure component update is handle by UI thread
        await InvokeAsync(() =>
        {
            StateHasChanged();
            Log.Warning($"Re-render '{PageName}'.");
        });
    }
}
