using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public class RootCauseTabulationStackBase : ReportTabulationStackBase
{
    [Parameter] public List<RootCauseReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<RootCauseReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<RootCauseReportModel> LowerBoundaryModels { get; set; } = [];
}
