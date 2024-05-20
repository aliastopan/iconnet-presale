using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Tabulations;

public partial class RootCauseClassificationTabulation : ComponentBase
{
    [Parameter]
    public List<RootCauseClassificationReportModel> Models { get; set; } = [];

    [Parameter]
    public bool IsPageView { get; set; }
}
