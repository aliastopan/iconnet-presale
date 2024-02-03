using System.Linq.Expressions;

namespace IConnet.Presale.WebApp.Helpers;

public abstract class ColumnWidthBase<T>
{
    private readonly static int _charWidth = 8;         //px
    private readonly static int _padding = 32;          //px
    private readonly static int _defaultWidth = 200;    //px

    public static int DefaultWidth => _defaultWidth;
    public int NamaPemohonPx { get; set; } = DefaultWidth;
    public int EmailPemohonPx { get; set; } = DefaultWidth;
    public int AlamatPemohonPx { get; set; } = DefaultWidth;
    public int NamaAgenPx { get; set; } = DefaultWidth;
    public int EmailAgenPx { get; set; } = DefaultWidth;
    public int MitraAgenPx { get; set; } = DefaultWidth;
    public int KeteranganPx { get; set; } = DefaultWidth;

    protected int CharWidth => _charWidth;
    protected int Padding => _padding;

    public string NamaPemohonStyle => $"width: {NamaPemohonPx}px;";
    public string EmailPemohonStyle => $"width: {EmailPemohonPx}px;";
    public string AlamatPemohonStyle => $"width: {AlamatPemohonPx}px;";
    public string NamaAgenStyle => $"width: {NamaAgenPx}px;";
    public string EmailAgenStyle => $"width: {EmailAgenPx}px;";
    public string MitraAgenStyle => $"width: {MitraAgenPx}px;";
    public string KeteranganStyle => $"width: {KeteranganPx}px;";

    public abstract void SetColumnWidth(IQueryable<T>? models);

    protected void SetColumnWidth(IQueryable<T> importModels, Expression<Func<T, int>> propertySelector, Action<int>
        setProperty, string propertyName)
    {
        if (importModels is null || !importModels.Any())
        {
            return;
        }

        int contentWidth = importModels.Max(propertySelector.Compile());
        int columnWidthPx = (contentWidth * CharWidth) + Padding;
        Log.Warning("{0} length: {1}, width: {2}px", propertyName, contentWidth, columnWidthPx);

        if (columnWidthPx > DefaultWidth)
        {
            setProperty(columnWidthPx);
        }
    }
}
