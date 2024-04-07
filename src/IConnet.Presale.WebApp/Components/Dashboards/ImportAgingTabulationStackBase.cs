using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public class ImportAgingTabulationStackBase : ReportTabulationStackBase
{
    [Parameter] public List<ImportAgingReportModel> UpperBoundaryModels { get; set; } = [];
    [Parameter] public List<ImportAgingReportModel> MiddleBoundaryModels { get; set; } = [];
    [Parameter] public List<ImportAgingReportModel> LowerBoundaryModels { get; set; } = [];
}
