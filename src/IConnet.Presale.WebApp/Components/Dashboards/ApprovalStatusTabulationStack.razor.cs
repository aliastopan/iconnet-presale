using Microsoft.AspNetCore.Components.Web;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public partial class ApprovalStatusTabulationStack : ComponentBase
{
    [Parameter]
    public List<ApprovalStatusReportModel> UpperBoundaryModels { get; set; } = [];

    [Parameter]
    public List<ApprovalStatusReportModel> MiddleBoundaryModels { get; set; } = [];

    [Parameter]
    public List<ApprovalStatusReportModel> LowerBoundaryModels { get; set; } = [];

    public bool IsMonthlySelected { get; set; } = false;
    public bool IsWeeklySelected { get; set; } = false;
    public bool IsDailySelected { get; set; } = false;

    protected override void OnInitialized()
    {
        IsMonthlySelected = true;
    }

    protected void ToggleToMonthlyView(MouseEventArgs _)
    {
        LogSwitch.Debug("Monthly");

        IsMonthlySelected = true;
        IsWeeklySelected = false;
        IsDailySelected = false;
    }

    protected void ToggleToWeeklyView(MouseEventArgs _)
    {
        LogSwitch.Debug("Weekly");

        IsMonthlySelected = false;
        IsWeeklySelected = true;
        IsDailySelected = false;
    }

    protected void ToggleToDailyView(MouseEventArgs _)
    {
        LogSwitch.Debug("Daily");

        IsMonthlySelected = false;
        IsWeeklySelected = false;
        IsDailySelected = true;
    }
}
