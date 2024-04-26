using System.Text.RegularExpressions;

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

    public static string SanitizeOnlyAlphanumeric(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        return new string(input.Where(char.IsLetterOrDigit).ToArray());
    }

    public static string SanitizeOnlyAlphanumericAndSpaces(this string input)
    {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

        return new string(input.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());
    }

    public static string CapitalizeFirstLetterOfEachWord(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var words = input.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 1)
            {
                if (words[i].ToUpper() == words[i])
                {
                    // word is an abbreviation, leave it as is
                    continue;
                }

                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
        }

        return string.Join(" ", words);
    }

    public static string RemoveNewlines(this string str)
    {
        if (str.Contains('\n') || str.Contains('\r'))
        {
            str = str.Replace("\n", "").Replace("\r", "");
        }

        return str;
    }

    public static string FormatHtmlBreak(this string input)
    {
        return input.Replace("\n", "<br />");
    }

    public static string FormatHtmlBold(this string input)
    {
        return Regex.Replace(input, @"\*(.*?)\*", "<b>$1</b>");
    }

    public static string FormatHtmlItalic(this string input)
    {
        return Regex.Replace(input, @"_(\w+)_", "<i>$1</i>");
    }

    public static string ReplacePlaceholder(this string input, string placeholder, string value)
    {
        return input.Replace(placeholder, value);
    }

    public static bool IsParsableAsInteger(this string input)
    {
        return int.TryParse(input, out int result);
    }

    public static string TruncateWithEllipsis(this string input, int maxLength = 24)
    {
        if (input.Length <= maxLength)
        {
            return input;
        }

        return input.Substring(0, maxLength - 3) + "...";
    }

    public static string ReplaceSpacesWithUnderscores(this string input)
    {
        return input.Replace(' ', '_');
    }
}
