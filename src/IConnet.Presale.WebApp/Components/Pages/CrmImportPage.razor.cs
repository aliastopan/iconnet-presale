namespace IConnet.Presale.WebApp.Components.Pages;

public partial class CrmImportPage
{
    [Inject] public IJSRuntime JsRuntime { get; init; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public CrmImportService CrmImportService { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; init; } = default!;

    private const int _itemPerPage = 10;
    private readonly PaginationState _pagination = new PaginationState { ItemsPerPage = _itemPerPage };

    private IQueryable<IApprovalOpportunityModel>? _importModels;

    private bool _isLoading = false;
    private int _importCount = 0;
    private CrmImportMetadata _importMetadata = default!;

    public bool EnableMessageBar => _importModels is not null && _importMetadata is not null;

    private async Task CrmImportAsync()
    {
        _isLoading = true;

        string clipboard = await PasteClipboardAsync();

        (List<IApprovalOpportunityModel> importModels, CrmImportMetadata importMetadata) result;
        result = await CrmImportService.ImportAsync(clipboard);

        _importModels = CrmImportService.GetApprovalOpportunities();;

        _importCount = await WorkloadManager.CacheWorkloadAsync(result.importModels);
        _importMetadata = result.importMetadata;

        _importMetadata.NumberOfDuplicates = _importMetadata.NumberOfRows - _importCount;

        if (_importMetadata.IsValidImport && _importCount > 0)
        {
            var message = $"{_importCount} CRM data has been imported.";
            await BroadcastService.BroadcastMessageAsync(message);
        }

        _isLoading = false;
    }

    private async Task<string> PasteClipboardAsync()
    {
        return await JsRuntime.InvokeAsync<string>("navigator.clipboard.readText");
    }
}
