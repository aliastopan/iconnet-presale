namespace IConnet.Presale.WebApp.Components.Pages;

public class CrmImportPageBase : ComponentBase
{
    [Inject] public IJSRuntime JsRuntime { get; init; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; init; } = default!;
    [Inject] public CrmImportService CrmImportService { get; init; } = default!;

    private const int _itemPerPage = 10;
    private readonly PaginationState _pagination = new PaginationState { ItemsPerPage = _itemPerPage };
    private IQueryable<IApprovalOpportunityModel>? _importModels;
    private CrmImportMetadata _importMetadata = default!;

    protected PaginationState Pagination => _pagination;
    protected IQueryable<IApprovalOpportunityModel>? ImportModels => _importModels;
    protected CrmImportMetadata ImportMetadata => _importMetadata;
    protected int ImportCount { get; set; }
    protected bool EnableMessageBar => _importModels is not null && _importMetadata is not null;
    protected bool IsLoading { get; set; } = false;

    protected async Task CrmImportAsync()
    {
        IsLoading = true;

        string clipboard = await PasteClipboardAsync();

        (List<IApprovalOpportunityModel> importModels, CrmImportMetadata importMetadata) result;
        result = await CrmImportService.ImportAsync(clipboard);

        _importModels = CrmImportService.GetApprovalOpportunities();;

        ImportCount = await WorkloadManager.CacheWorkloadAsync(result.importModels);
        _importMetadata = result.importMetadata;

        _importMetadata.NumberOfDuplicates = _importMetadata.NumberOfRows - ImportCount;

        if (_importMetadata.IsValidImport && ImportCount > 0)
        {
            var message = $"{ImportCount} CRM data has been imported.";
            await BroadcastService.BroadcastMessageAsync(message);
        }

        IsLoading = false;
    }

    protected async Task<string> PasteClipboardAsync()
    {
        return await JsRuntime.InvokeAsync<string>("navigator.clipboard.readText");
    }
}
