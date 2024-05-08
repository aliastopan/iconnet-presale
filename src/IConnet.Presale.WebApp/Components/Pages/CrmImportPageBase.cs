using IConnet.Presale.WebApp.Components.Dialogs;

namespace IConnet.Presale.WebApp.Components.Pages;

public class CrmImportPageBase : ComponentBase, IPageNavigation
{
    [Inject] public IJSRuntime JsRuntime { get; init; } = default!;
    [Inject] public TabNavigationManager TabNavigationManager { get; set; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; init; } = default!;
    [Inject] public CrmImportService CrmImportService { get; init; } = default!;
    [Inject] public CsvParserService CsvParserService { get; set; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;
    [Inject] public IDialogService DialogService { get; set; } = default!;

    private readonly ImportModelColumnWidth _columnWidth = new();
    private const int _itemPerPage = 10;
    private readonly PaginationState _pagination = new() { ItemsPerPage = _itemPerPage };
    private HashSet<string> _duplicateIds = new();
    private IQueryable<IApprovalOpportunityModel>? _importModels;
    private CrmImportMetadata _importMetadata = default!;

    protected ImportModelColumnWidth ColumnWidth => _columnWidth;
    protected bool IsPageScrollDataGrid { get; set; } = false;
    protected bool EnablePageScrollToggle => _pagination.ItemsPerPage <= 10;
    protected string PaginationItemsPerPageOptions { get; set ;} = default!;
    protected PaginationState Pagination => _pagination;
    protected HashSet<string> DuplicateIds => _duplicateIds;
    protected IQueryable<IApprovalOpportunityModel>? ImportModels => _importModels;
    protected CrmImportMetadata ImportMetadata => _importMetadata;
    protected int ImportCount { get; set; }
    protected bool IsLoading { get; set; } = false;

    protected string GridTemplateCols => GetGridTemplateCols();

    private const int _byte = 1024;
    private const int _megabyte = 1024 * _byte;
    private bool _isDownloadingTemplate;

    protected int MaxFileSize => 10 * _megabyte;
    protected FluentInputFile? FileUploader { get; set; } = default!;
    protected FluentInputFileEventArgs[] Files { get; set; } = Array.Empty<FluentInputFileEventArgs>();
    protected int? ProgressPercent { get; set; }
    protected string? ProgressTitle { get; set;}

    public TabNavigationModel PageDeclaration()
    {
        return new TabNavigationModel("crm-import", PageNavName.CrmImport, PageRoute.CrmImport);
    }

    protected override void OnInitialized()
    {
        TabNavigationManager.SelectTab(this);

        base.OnInitialized();
    }

    protected async Task CrmImportAsync()
    {
        IsLoading = true;

        string clipboard = await PasteClipboardAsync();

        (List<IApprovalOpportunityModel> importModels, CrmImportMetadata importMetadata) result;
        result = await CrmImportService.ImportAsync(clipboard);

        _importModels = CrmImportService.GetApprovalOpportunities();

        var (importCount , duplicateIds) = await WorkloadManager.InsertWorkloadAsync(result.importModels);

        ImportCount = importCount;
        _duplicateIds = duplicateIds;

        _importMetadata = result.importMetadata;

        _importMetadata.NumberOfDuplicates = _importMetadata.NumberOfRows - ImportCount;

        if (_importMetadata.IsValidImport && ImportCount > 0)
        {
            var broadcastMessage = $"{ImportCount} CRM data has been imported.";
            await BroadcastService.BroadcastMessageAsync(broadcastMessage);
        }

        _columnWidth.SetColumnWidth(ImportModels);

        IsLoading = false;
        ImportResultToast();
    }

    protected async Task<string> PasteClipboardAsync()
    {
        return await JsRuntime.InvokeAsync<string>("readClipboard");
    }

    protected bool HasDuplicate(string idPermohonan)
    {
        return DuplicateIds.Contains(idPermohonan);
    }

    protected async Task OpenImportGuideDialogAsync()
    {
        var parameters = new DialogParameters()
        {
            Title = "Petujuk Upload Import",
            TrapFocus = true,
            Width = "650px",
        };

        var dialog = await DialogService.ShowDialogAsync<CsvImportGuideDialog>(parameters);
        // var result = await dialog.Result;
    }

    protected async Task UploadAsync(IEnumerable<FluentInputFileEventArgs> files)
    {
        IsLoading = true;

        Files = files.ToArray();
        ProgressPercent = FileUploader!.ProgressPercent;
        ProgressTitle = FileUploader!.ProgressTitle;

        if (Files.Length == 0)
        {
            IsLoading = false;

            return;
        }

        FluentInputFileEventArgs fileInput = Files.First();
        FileInfo fileInfo = fileInput.LocalFile!;

        if (fileInput.Size > MaxFileSize)
        {
            FileExceedingLimitToast();
            IsLoading = false;

            return;
        }

        if (fileInput.ContentType != "text/csv")
        {
            UploadResultToast(fileInput, isSuccess: false);
            IsLoading = false;

            return;
        }
        else
        {
            UploadResultToast(fileInput, isSuccess: true);
        }

        bool IsFileCsv = CsvParserService.TryGetCsvFromLocal(fileInfo, out List<string[]>? csv, out string errorMessage);

        if (!IsFileCsv || csv is null)
        {
            FailedCsvParsingToast(errorMessage);
            IsLoading = false;

            return;
        }

        (List<IApprovalOpportunityModel> importModels, CrmImportMetadata importMetadata) result;
        result = await CrmImportService.ImportFromCsvAsync(csv);

        _importModels = CrmImportService.GetApprovalOpportunities();

        var (importCount , duplicateIds) = await WorkloadManager.InsertWorkloadAsync(result.importModels);

        ImportCount = importCount;

        _duplicateIds = duplicateIds;
        _importMetadata = result.importMetadata;
        _importMetadata.NumberOfDuplicates = _duplicateIds.Count;

        if (_importMetadata.IsValidImport && ImportCount > 0)
        {
            var broadcastMessage = $"{ImportCount} CRM data has been imported.";
            await BroadcastService.BroadcastMessageAsync(broadcastMessage);
        }

        _columnWidth.SetColumnWidth(ImportModels);

        IsLoading = false;
        ImportResultToast();

        // clean-up temp folder
        foreach (var any in Files)
        {
            any.LocalFile?.Delete();
        }
    }

    protected async Task DownloadImportTemplateAsync()
    {
        if (_isDownloadingTemplate)
        {
            return;
        }

        _isDownloadingTemplate = true;

        string fileName = $"PresaleApp_Import_Template.xlsx";

        DownloadImportTemplateToast(fileName);

        byte[] xlsxBytes = CsvParserService.GenerateXlsxImportTemplateBytes();
        string base64 = Convert.ToBase64String(xlsxBytes);

        await JsRuntime.InvokeVoidAsync("downloadFile", fileName, base64);

        _isDownloadingTemplate = false;
    }

    private void DownloadImportTemplateToast(string fileName)
    {
        var intent = ToastIntent.Download;
        var message = $"Generating {fileName}";

        ToastService.ShowToast(intent, message);
    }

    private void UploadResultToast(FluentInputFileEventArgs fileInput, bool isSuccess)
    {
        if (isSuccess)
        {
            var intent = ToastIntent.Upload;
            var fileSize = $"{Decimal.Divide(fileInput.Size, 1024):N} KB";
            var message = $"Upload {fileSize} '{fileInput.Name}'";

            ToastService.ShowToast(intent, message);
        }
        else
        {
            var intent = ToastIntent.Error;
            var message = $"Invalid. Upload import harus berekstensikan .csv";

            ToastService.ShowToast(intent, message);
        }
    }

    private void FileExceedingLimitToast()
    {
        var intent = ToastIntent.Error;
        var message = "Proses import .csv gagal. File melebihi ukuran batas 10MB.";

        ToastService.ShowToast(intent, message);
    }

    private void FailedCsvParsingToast(string errorMessage)
    {
        var intent = ToastIntent.Error;
        var message = $"Proses import .csv gagal. {errorMessage}";
        var timeout = 15000;

        ToastService.ShowToast(intent, message, timeout: timeout);
    }

    private void ImportResultToast()
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

    protected string GetWidthStyle(int widthPx)
    {
        return $"width: {widthPx}px;";
    }

    protected void OnItemsPerPageChanged(string ItemsPerPageString)
    {
        int itemsPerPage = int.Parse(ItemsPerPageString);
        _pagination.ItemsPerPage = itemsPerPage;
    }

    private string GetGridTemplateCols()
    {
        return $@"
            {ColumnWidth.IdPermohonanPx}px
            {ColumnWidth.TglPermohonanPx}px
            {ColumnWidth.DurasiTidakLanjutPx}px
            {ColumnWidth.NamaPemohonPx}px
            {ColumnWidth.IdPlnPx}px
            {ColumnWidth.LayananPx}px
            {ColumnWidth.SumberPermohonanPx}px
            {ColumnWidth.StatusPermohonanPx}px
            {ColumnWidth.NamaAgenPx}px
            {ColumnWidth.EmailAgenPx}px
            {ColumnWidth.TelpAgenPx}px
            {ColumnWidth.MitraAgenPx}px
            {ColumnWidth.SplitterPx}px
            {ColumnWidth.TelpPemohonPx}px
            {ColumnWidth.EmailPemohonPx}px
            {ColumnWidth.NikPemohonPx}px
            {ColumnWidth.NpwpPemohonPx}px
            {ColumnWidth.KeteranganPx}px
            {ColumnWidth.AlamatPemohonPx}px
            {ColumnWidth.RegionalPx}px
            {ColumnWidth.KantorPerwakilanPx}px
            {ColumnWidth.ProvinsiPx}px
            {ColumnWidth.KabupatenPx}px
            {ColumnWidth.KecamatanPx}px
            {ColumnWidth.KelurahanPx}px
            {ColumnWidth.LatitudePx}px
            {ColumnWidth.LongitudePx}px;";
    }

    protected string GetPaginationStyle()
    {
        if (_pagination.ItemsPerPage <= 10 || !IsPageScrollDataGrid)
        {
            return "max-height: 364px !important";
        }

        return "min-height: 364px !important";
    }
}
