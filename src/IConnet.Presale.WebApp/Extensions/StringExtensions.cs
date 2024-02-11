namespace IConnet.Presale.WebApp.Extensions;

public static class StringExtensions
{
    public static bool HasValue(this string? str)
    {
        return !string.IsNullOrEmpty(str) || !string.IsNullOrWhiteSpace(str);
    }
}
