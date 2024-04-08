using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public class ChatCallMulaiAgingTabulationStackBase : ReportTabulationStackBase
{
    [Parameter] public List<ChatCallMulaiAgingReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ChatCallMulaiAgingReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ChatCallMulaiAgingReportModel> LowerBoundaryModels { get; set; } = [];

    public bool IsPageView { get; set; }
}
