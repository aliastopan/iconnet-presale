using System.Globalization;

namespace IConnet.Presale.WebApp.Components.Custom.Badges;

public partial class DateTimeBadge : ComponentBase
{
    [Parameter]
    public DateTime DateTime { get; set; } = default!;

    public string GetDateOnly()
    {
        var cultureInfo = new CultureInfo("id-ID");
        return DateTime.ToString("dd MMM yyyy", cultureInfo);
    }

    public string GetTimeOnly()
    {
        return DateTime.ToString("HH:mm");
    }
}
