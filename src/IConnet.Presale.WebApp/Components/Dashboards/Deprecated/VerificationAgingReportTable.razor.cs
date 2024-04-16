using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Deprecated;

public partial class VerificationAgingReportTable
{
    [Parameter]
    public List<VerificationAgingReportModel> Models { get; set; } = [];
}
