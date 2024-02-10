using System.Linq.Expressions;

namespace IConnet.Presale.WebApp.Helpers;

public abstract class ColumnWidthBase<T>
{
    private readonly static int _charWidth = 8;         //px
    private readonly static int _padding = 16;          //px
    private readonly static int _defaultWidth = 200;    //px

    public static int DefaultWidth => _defaultWidth;

    public int IdPermohonanPx { get; set; } = 185;
    public int TglPermohonanPx { get; set; } = DefaultWidth;
    public int DurasiTidakLanjutPx { get; set; } = DefaultWidth;
    public int NamaPemohonPx { get; set; } = DefaultWidth;
    public int IdPlnPx { get; set; } = 135;
    public int LayananPx { get; set; } = 150;
    public int SumberPermohonanPx { get; set; } = 210;
    public int StatusPermohonanPx { get; set; } = 210;
    public int NamaAgenPx { get; set; } = DefaultWidth;
    public int EmailAgenPx { get; set; } = DefaultWidth;
    public int TelpAgenPx { get; set; } = 150;
    public int MitraAgenPx { get; set; } = DefaultWidth;
    public int SplitterPx { get; set; } = 165;
    public int JenisPermohonanPx { get; set; } = 180;
    public int TelpPemohonPx { get; set; } = 150;
    public int EmailPemohonPx { get; set; } = DefaultWidth;
    public int NikPemohonPx { get; set; } = 150;
    public int NpwpPemohonPx { get; set; } = 150;
    public int KeteranganPx { get; set; } = DefaultWidth;
    public int AlamatPemohonPx { get; set; } = DefaultWidth;
    public int RegionalPx { get; set; } = 150;
    public int KantorPerwakilanPx { get; set; } = 180;
    public int ProvinsiPx { get; set; } = 150;
    public int KabupatenPx { get; set; } = 150;
    public int KecamatanPx { get; set; } = 150;
    public int KelurahanPx { get; set; } = 150;
    public int LatitudePx { get; set; } = 150;
    public int LongitudePx { get; set; } = 150;

    public int ImportSignaturePx { get; set; } = DefaultWidth;

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

        int contentWidth = importModels.Max(propertySelector.Compile());
        int charWidth = isCapitalized ? CharWidth + (int)(CharWidth * 0.25f) : CharWidth;
        int columnWidthPx = (contentWidth * charWidth) + Padding;
        Log.Warning("{0} length: {1}, width: {2}px", propertyName, contentWidth, columnWidthPx);

        if (columnWidthPx > DefaultWidth)
        {
            setProperty(columnWidthPx);
        }
    }
}
