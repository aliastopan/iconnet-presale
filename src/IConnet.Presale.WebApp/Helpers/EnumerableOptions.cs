namespace IConnet.Presale.WebApp.Helpers;

public static class EnumerableOptions
{
    private static List<string> _kantorPerwakilan =
    [
        "SEMUA KANTOR PERWAKILAN",
        "SURABAYA",
        "JEMBER",
        "MADIUN",
        "MALANG"
    ];

    private static List<string> _filterSearchType =
    [
        "ID PERMOHONAN",
    ];

    public static IEnumerable<string> KantorPerwakilan => _kantorPerwakilan;
    public static IEnumerable<string> Filter => _filterSearchType;
}
