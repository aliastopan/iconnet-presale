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

    public static IEnumerable<string> KantorPerwakilan => _kantorPerwakilan;
}
