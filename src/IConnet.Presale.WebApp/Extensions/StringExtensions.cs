namespace IConnet.Presale.WebApp.Extensions;

public static class StringExtensions
{
    public static bool HasValue(this string? str)
    {
        return !string.IsNullOrEmpty(str) || !string.IsNullOrWhiteSpace(str);
    }

    public static bool IsNullOrWhiteSpace(this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    public static string RemoveNewlines(this string str)
    {
        if (str.Contains("\n") || str.Contains("\r"))
        {
            str = str.Replace("\n", "").Replace("\r", "");
        }

        return str;
    }

    public static string FormatHtmlBreak(this string input)
    {
        return input.Replace("\n", "<br />");
    }
}
