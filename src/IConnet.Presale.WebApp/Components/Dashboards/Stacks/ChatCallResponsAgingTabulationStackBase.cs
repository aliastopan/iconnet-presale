using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Stacks;

public class ChatCallResponsAgingTabulationStackBase : ReportTabulationStackBase
{
    [Parameter] public List<ChatCallResponsAgingReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ChatCallResponsAgingReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ChatCallResponsAgingReportModel> LowerBoundaryModels { get; set; } = [];

    public bool IsPageView { get; set; }
}
