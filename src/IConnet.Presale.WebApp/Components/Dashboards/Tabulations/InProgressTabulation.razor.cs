using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Tabulations;

public partial class InProgressTabulation : ComponentBase
{
    [Parameter]
    public List<InProgressReportModel> Models { get; set; } = [];
}
