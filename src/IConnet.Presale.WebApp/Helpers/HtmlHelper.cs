namespace IConnet.Presale.WebApp.Helpers;

public static class HtmlHelper
{
    private static readonly Icon _questionIcon = new Icons.Filled.Size20.QuestionCircle();
    private static readonly Icon _errorIcon = new Icons.Filled.Size20.ErrorCircle();
    private static readonly Icon _checkmarkIcon = new Icons.Filled.Size20.CheckmarkCircle();

    public static Icon GetFluentIcon(string? section,
        string questionIconColor = "var(--info)",
        string errorIconColor = "var(--error)",
        string checkmarkIconColor = "var(--success)")
    {
        return section switch
        {
            string status when status == OptionSelect.StatusValidasi.TidakSesuai => _errorIcon.WithColor(errorIconColor),
            string status when status == OptionSelect.StatusValidasi.Sesuai => _checkmarkIcon.WithColor(checkmarkIconColor),
            _ => _questionIcon.WithColor(questionIconColor),
        };
    }

    public static string GetCssBackgroundColor(string? section,
        string waiting = "validation-value-bg-waiting",
        string invalid = "validation-value-bg-invalid",
        string valid = "validation-value-bg-valid")
    {
        var css = section switch
        {
            string status when status == OptionSelect.StatusValidasi.TidakSesuai => invalid,
            string status when status == OptionSelect.StatusValidasi.Sesuai => valid,
            _ => waiting,
        };

        return $"{css} validation-value-clipboard";
    }
}
