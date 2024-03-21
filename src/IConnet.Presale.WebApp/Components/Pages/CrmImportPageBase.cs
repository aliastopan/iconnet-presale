namespace IConnet.Presale.WebApp.Components.Pages;

public class CrmImportPageBase : ComponentBase, IPageNavigation
{
    [Inject] public IJSRuntime JsRuntime { get; init; } = default!;
    [Inject] public TabNavigationManager TabNavigationManager { get; set; } = default!;
    [Inject] public IWorkloadManager WorkloadManager { get; init; } = default!;
    [Inject] public BroadcastService BroadcastService { get; init; } = default!;
    [Inject] public CrmImportService CrmImportService { get; init; } = default!;
    [Inject] public IToastService ToastService { get; set; } = default!;

    private readonly ImportModelColumnWidth _columnWidth = new();
    private const int _itemPerPage = 10;
    private readonly PaginationState _pagination = new() { ItemsPerPage = _itemPerPage };
    private HashSet<string> _duplicateIds = new();
    private IQueryable<IApprovalOpportunityModel>? _importModels;
    private CrmImportMetadata _importMetadata = default!;

    protected ImportModelColumnWidth ColumnWidth => _columnWidth;
    protected string PaginationItemsPerPageOptions { get; set ;} = default!;
    protected PaginationState Pagination => _pagination;
    protected HashSet<string> DuplicateIds => _duplicateIds;
    protected IQueryable<IApprovalOpportunityModel>? ImportModels => _importModels;
    protected CrmImportMetadata ImportMetadata => _importMetadata;
    protected int ImportCount { get; set; }
    protected bool IsLoading { get; set; } = false;

    protected string GridTemplateCols => GetGridTemplateCols();

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

        _importModels = CrmImportService.GetApprovalOpportunities();;

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
            {ColumnWidth.JenisPermohonanPx}px
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
        if (_pagination.ItemsPerPage <= 10)
        {
            return "max-height: 364px !important";
        }

        return "min-height: 364px !important";
    }
}
