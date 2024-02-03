namespace IConnet.Presale.WebApp.Components.Pages;

public class CrmImportPageBase : ComponentBase
{
    [Inject] public IJSRuntime JsRuntime { get; init; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; init; } = default!;
    [Inject] public CrmImportService CrmImportService { get; init; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;

    private readonly ImportModelColumnWidth _columnWidth = new();
    private const int _itemPerPage = 10;
    private readonly PaginationState _pagination = new() { ItemsPerPage = _itemPerPage };
    private IQueryable<IApprovalOpportunityModel>? _importModels;
    private CrmImportMetadata _importMetadata = default!;

    protected ImportModelColumnWidth ColumnWidth => _columnWidth;
    protected PaginationState Pagination => _pagination;
    protected IQueryable<IApprovalOpportunityModel>? ImportModels => _importModels;
    protected CrmImportMetadata ImportMetadata => _importMetadata;
    protected int ImportCount { get; set; }
    protected bool IsLoading { get; set; } = false;

    protected string GridTemplateCols
    {
        get => $@"150px 180px 200px
            {ColumnWidth.NamaPemohonPx}px 120px 150px 200px 200px
            {ColumnWidth.NamaAgenPx}px
            {ColumnWidth.EmailAgenPx}px 150px
            {ColumnWidth.MitraAgenPx}px 150px 200px 200px
            {ColumnWidth.EmailPemohonPx}px 150px 150px
            {ColumnWidth.KeteranganPx}px
            {ColumnWidth.AlamatPemohonPx}px 180px 180px 150px 150px 150px 150px 150px 150px;";
    }

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

        _columnWidth.SetColumnWidth(ImportModels);

        IsLoading = false;
        ImportResultNotification();
    }

    protected async Task<string> PasteClipboardAsync()
    {
        return await JsRuntime.InvokeAsync<string>("navigator.clipboard.readText");
    }

    private void ImportResultNotification()
    {
        if (ImportCount > 0)
        {
            var intent = ToastIntent.Success;
            var message = $"{ImportCount} dari {ImportMetadata.NumberOfRows} data telah berhasil di-import.";
            ToastService.ShowToast(intent, message);
        }

        if (!ImportMetadata.IsValidImport)
        {
            var intent = ToastIntent.Error;
            var message = "Proses import gagal. Jumlah kolom dan baris tidak sesuai.";
            ToastService.ShowToast(intent, message);
        }

        if (ImportMetadata!.NumberOfDuplicates > 0)
        {
            var intent = ToastIntent.Warning;
            var message = $"Terdapat {ImportMetadata.NumberOfDuplicates} duplikasi saat dalam proses copy-paste dari iCRM+.";
            ToastService.ShowToast(intent, message);
        }
    }
}
