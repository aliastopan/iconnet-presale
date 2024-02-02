namespace IConnet.Presale.WebApp.Helpers;

public class ColumnWidthBase
{
    private static readonly int _charWidth = 8; //px
    private static readonly int _padding = 20;  //px

    public int NamaPemohonPx { get; set; } = 200;
    public int EmailPemohonPx { get; set; } = 200;
    public int AlamatPemohonPx { get; set; } = 200;
    public int NamaAgenPx { get; set; } = 200;
    public int EmailAgenPx { get; set; } = 200;
    public int MitraAgenPx { get; set; } = 200;
    // KeteranganPx

    protected int CharWidth => _charWidth;
    protected int Padding => _padding;

    public string NamaPemohonStyle => $"width: {NamaPemohonPx}px;";
    public string EmailPemohonStyle => $"width: {EmailPemohonPx}px;";
    public string AlamatPemohonStyle => $"width: {AlamatPemohonPx}px;";
    public string NamaAgenStyle => $"width: {NamaAgenPx}px;";
    public string EmailAgenStyle => $"width: {EmailAgenPx}px;";
    public string MitraAgenStyle => $"width: {MitraAgenPx}px;";
}
