using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Deprecated;

public partial class ImportAgingReportTable : ComponentBase
{
    [Parameter]
    public List<ImportAgingReportModel> Models { get; set; } = [];
}
