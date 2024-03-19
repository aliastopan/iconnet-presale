using System.Linq.Expressions;

namespace IConnet.Presale.WebApp.Helpers;

public abstract class ColumnWidthBase<T>
{
    private readonly static int _charWidth = 8;         //px
    private readonly static int _padding = 16;          //px
    private readonly static int _defaultWidth = 200;    //px

    public static int DefaultWidth => _defaultWidth;
    public int OffsetPx => 32;

    public int WorkPaperLevelPx { get; set; } = DefaultWidth;
    public int IdPermohonanPx { get; set; } = DefaultWidth;
    public int TglPermohonanPx { get; set; } = DefaultWidth;
    public int DurasiTidakLanjutPx { get; set; } = DefaultWidth;
    public int NamaPemohonPx { get; set; } = DefaultWidth;
    public int IdPlnPx { get; set; } = DefaultWidth - 65;
    public int LayananPx { get; set; } = DefaultWidth - 50;
    public int SumberPermohonanPx { get; set; } = DefaultWidth + 10;
    public int StatusPermohonanPx { get; set; } = DefaultWidth + 10;
    public int NamaAgenPx { get; set; } = DefaultWidth;
    public int EmailAgenPx { get; set; } = DefaultWidth;
    public int TelpAgenPx { get; set; } = DefaultWidth - 20;
    public int MitraAgenPx { get; set; } = DefaultWidth;
    public int SplitterPx { get; set; } = DefaultWidth - 35;
    public int JenisPermohonanPx { get; set; } = DefaultWidth - 20;
    public int TelpPemohonPx { get; set; } = DefaultWidth - 20;
    public int EmailPemohonPx { get; set; } = DefaultWidth;
    public int NikPemohonPx { get; set; } = DefaultWidth - 20;
    public int NpwpPemohonPx { get; set; } = DefaultWidth - 20;
    public int KeteranganPx { get; set; } = DefaultWidth;
    public int AlamatPemohonPx { get; set; } = DefaultWidth;
    public int RegionalPx { get; set; } = DefaultWidth - 50;
    public int KantorPerwakilanPx { get; set; } = DefaultWidth;
    public int ProvinsiPx { get; set; } = DefaultWidth - 50;
    public int KabupatenPx { get; set; } = DefaultWidth -50;
    public int KecamatanPx { get; set; } = DefaultWidth - 50;
    public int KelurahanPx { get; set; } = DefaultWidth - 50;
    public int LatitudePx { get; set; } = DefaultWidth - 50;
    public int LongitudePx { get; set; } = DefaultWidth - 50;

    public int InChargeImportPx { get; set; } = DefaultWidth;
    public int StagingStatusPx { get; set; } = DefaultWidth;

    public int HelpdeskInChargePx { get; set; } = DefaultWidth;
    public int ShiftPx { get; set; } = 100;
    public int TglChatCallMulaiPx { get; set; } = DefaultWidth;
    public int InChargeChatCallMulaiPx { get; set; } = DefaultWidth;
    public int TglChatCallResponsPx { get; set; } = DefaultWidth + 15;
    public int InChargeChatCallResponsPx { get; set; } = DefaultWidth + 15;
    public int ValidasiNamaPelangganPx { get; set; } = DefaultWidth;
    public int ValidasiNomorTelpPx { get; set; } = DefaultWidth;
    public int ValidasiEmailPx { get; set; } = DefaultWidth;
    public int ValidasiAlamatPx { get; set; } = DefaultWidth;
    public int ValidasiIdPlnPx { get; set; } = DefaultWidth;
    public int ValidasiShareLocPx { get; set; } = DefaultWidth;
    public int LinkChatHistoryPx { get; set; } = DefaultWidth + 50;
    public int StatusValidasiPx { get; set; } = DefaultWidth;
    public int KeteranganValidasiPx { get; set; } = DefaultWidth;
    public int ContactWhatsAppPx { get; set; } = DefaultWidth - 20;

    public int PlanningAssetCoverageInChargePx { get; set; } = DefaultWidth;
    public int StatusApprovalPx { get; set; } = DefaultWidth;
    public int RootCausePx { get; set; } = DefaultWidth;
    public int SplitterGantiPx { get; set; } = DefaultWidth - 35;
    public int TglApprovalPx { get; set; } = DefaultWidth;
    public int KeteranganApprovalPx { get; set; } = DefaultWidth;

    protected int CharWidth => _charWidth;
    protected int Padding => _padding;

    public abstract void SetColumnWidth(IQueryable<T>? models);

    protected void SetColumnWidth(IQueryable<T> importModels, Expression<Func<T, int>> propertySelector, Action<int>
        setProperty, string propertyName, bool isCapitalized = false)
    {
        if (importModels is null || !importModels.Any())
        {
            return;
        }

        int extraPx = 1;
        int contentWidth = importModels.Max(propertySelector.Compile());
        int charWidth = isCapitalized ? CharWidth + extraPx : CharWidth;
        int columnWidthPx = (contentWidth * charWidth) + Padding;

        // LogSwitch.Debug("{0} length: {1}, width: {2}px", propertyName, contentWidth, columnWidthPx);
        setProperty(columnWidthPx);

        if (columnWidthPx <= DefaultWidth)
        {
            setProperty(DefaultWidth);
        }
    }
}
