namespace IConnet.Presale.WebApp.Extensions;

public static class MarkupStringExtensions
{
    public static MarkupString ReplacePlaceholder(this MarkupString input, string placeholder, string value)
    {
        string inputString = input.ToString().Replace(placeholder, value);
        return new MarkupString(inputString);
    }
}
