namespace IConnet.Presale.WebApp.Components.Pages;

public class CrmImportPageBase : ComponentBase
{
    [Inject] public IJSRuntime JsRuntime { get; init; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; init; } = default!;
    [Inject] public CrmImportService CrmImportService { get; init; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;

    private const int _charWidth = 8; //px
    private const int _colsWidthPadding = 20; //px
    private const int _itemPerPage = 10;
    private readonly PaginationState _pagination = new PaginationState { ItemsPerPage = _itemPerPage };
    private IQueryable<IApprovalOpportunityModel>? _importModels;
    private CrmImportMetadata _importMetadata = default!;
    private static int _colWidthNamaPemohonPx = 200;
    private static int _colWidthEmailPemohonPx = 200;
    private static int _colWidthAlamatPemohonPx = 200;

    protected PaginationState Pagination => _pagination;
    protected IQueryable<IApprovalOpportunityModel>? ImportModels => _importModels;
    protected CrmImportMetadata ImportMetadata => _importMetadata;
    protected int ImportCount { get; set; }
    protected bool IsLoading { get; set; } = false;

    public string ColWidthNamaPemohonStyle => $"width: {_colWidthNamaPemohonPx}px;";
    public string ColWidthEmailPemohonStyle => $"width: {_colWidthEmailPemohonPx}px;";
    public string ColWidthAlamatPemohonStyle => $"width: {_colWidthAlamatPemohonPx}px;";
    public string GridTemplateCols
    {
        get => $"150px 180px 200px {_colWidthNamaPemohonPx}px 120px 150px 200px 200px 150px 150px 150px 150px 150px {_colWidthEmailPemohonPx}px 150px 150px 150px {_colWidthAlamatPemohonPx}px 180px 180px 150px 150px 150px 150px 150px 150px;";
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

        SetNamaPemohonColWidth();
        SetEmailPemohonColWidth();
        SetAlamatPemohonColWidth();

        IsLoading = false;
        ToastNotification();
    }

    protected async Task<string> PasteClipboardAsync()
    {
        return await JsRuntime.InvokeAsync<string>("navigator.clipboard.readText");
    }

    protected void ToastNotification()
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

    private void SetNamaPemohonColWidth()
    {
        if (ImportModels is null || !ImportModels.Any())
        {
            return;
        }

        var contentWidth = ImportModels!.Max(importModel => importModel.NamaPemohon.Length);
        _colWidthNamaPemohonPx = (contentWidth * _charWidth) + _colsWidthPadding;

        Log.Warning("Nama Pemohon col-width: {0}px", _colWidthNamaPemohonPx);

    }

    private void SetEmailPemohonColWidth()
    {
        if (ImportModels is null || !ImportModels.Any())
        {
            return;
        }

        var contentWidth = ImportModels!.Max(importModel => importModel.EmailPemohon.Length);
        _colWidthEmailPemohonPx = (contentWidth * _charWidth) + _colsWidthPadding;

        Log.Warning("Email Pemohon col-width: {0}px", _colWidthEmailPemohonPx);
    }

    private void SetAlamatPemohonColWidth()
    {
        if (ImportModels is null || !ImportModels.Any())
        {
            return;
        }

        var contentWidth = ImportModels!.Max(importModel => importModel.AlamatPemohon.Length);
        _colWidthAlamatPemohonPx = (contentWidth * _charWidth) + _colsWidthPadding;

        Log.Warning("Alamat Pemohon col-width: {0}px", _colWidthAlamatPemohonPx);
    }
}
