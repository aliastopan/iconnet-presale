namespace IConnet.Presale.WebApp.Components.Pages;

public class WorkloadPageBase : ComponentBase
{
    [Inject] public TabNavigationManager TabNavigationManager { get; set; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; init; } = default!;

    private bool _isInitialized = false;
    private const int _itemPerPage = 10;
    private readonly PaginationState _pagination = new PaginationState { ItemsPerPage = _itemPerPage };
    private readonly WorkloadColumnWidth _columnWidth = new WorkloadColumnWidth();

    private IQueryable<WorkPaper>? _workPapers;
    private readonly GridSort<WorkPaper> _sortByIdPermohonan = GridSort<WorkPaper>
        .ByAscending(workPaper => workPaper.ApprovalOpportunity.IdPermohonan);
    private readonly GridSort<WorkPaper> _sortByWorkPaperLevel = GridSort<WorkPaper>
        .ByAscending(workPaper => workPaper.WorkPaperLevel);

    protected string PaginationItemsPerPageOptions { get; set ;} = default!;
    protected PaginationState Pagination => _pagination;
    protected WorkloadColumnWidth ColumnWidth => _columnWidth;
    protected WorkloadFilter WorkloadFilter { get; set; } = WorkloadFilter.OnlyImportVerified;
    protected virtual IQueryable<WorkPaper>? WorkPapers => _workPapers;
    protected GridSort<WorkPaper> SortByIdPermohonan => _sortByIdPermohonan;
    protected GridSort<WorkPaper> SortByWorkPaperLevel => _sortByWorkPaperLevel;

    protected string PageName { get; set; } = "Workload page (base)";
    protected virtual bool IsLoading { get; set; } = false;

    protected override async Task OnInitializedAsync()
    {
        if (!_isInitialized)
        {
            _workPapers = await WorkloadManager.GetWorkloadAsync(WorkloadFilter);
            BroadcastService.Subscribe(OnUpdateWorkloadAsync);

            _isInitialized = true;
        }
    }

    protected virtual async Task OnUpdateWorkloadAsync(string message)
    {
        _workPapers = await WorkloadManager.GetWorkloadAsync(WorkloadFilter);

        // Log.Warning(message);

        // ensure component update is handle by UI thread
        await InvokeAsync(() =>
        {
            StateHasChanged();
            // Log.Warning($"Re-render '{PageName}'.");
        });
    }

    protected void OnItemsPerPageChanged(string ItemsPerPageString)
    {
        int itemsPerPage = int.Parse(ItemsPerPageString);
        _pagination.ItemsPerPage = itemsPerPage;
    }

    protected string GetWidthStyle(int widthPx, int offsetPx = 0)
    {
        return $"width: {widthPx + offsetPx}px;";
    }
}
