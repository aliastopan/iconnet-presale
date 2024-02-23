namespace IConnet.Presale.WebApp.Helpers;

public static class OptionSelect
{
    public static class StatusValidasi
    {
        public static IEnumerable<string> StatusValidasiOptions => EnumProcessor.GetStringOptions<ValidationStatus>();
        public static string MenungguValidasi => StatusValidasiOptions.First();
        public static string TidakSesuai => StatusValidasiOptions.Skip(1).First();
        public static string Sesuai => StatusValidasiOptions.Last();
    }
}
