using IConnet.Presale.WebApp.Components.Dashboards.Filters;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class InProgressTabulationStackBase : ReportTabulationStackBase
{
    protected override Appearance WeeklyToggleAppearance => IsWeeklySelected || IsMonthlySelected ? Appearance.Accent : Appearance.Neutral;
    protected override Appearance DailyToggleAppearance => base.DailyToggleAppearance;

    protected override string WeeklyToggleDisplayStyle => IsWeeklySelected || IsMonthlySelected ? "d-flex flex-column w-100" : "display: none";
    protected override string DailyToggleDisplayStyle => base.DailyToggleDisplayStyle;
}
