using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Tabulations;

public partial class ImportSlaTabulation
{
    [Parameter]
    public List<ImportAgingReportModel> Models { get; set; } = [];

    [Parameter]
    public bool IsPageView { get; set; }
}
