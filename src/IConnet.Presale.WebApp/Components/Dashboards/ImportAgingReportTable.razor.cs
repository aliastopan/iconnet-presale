using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public partial class ImportAgingReportTable : ComponentBase
{
    [Parameter]
    public List<ImportAgingReportModel> Models { get; set; } = [];
}
