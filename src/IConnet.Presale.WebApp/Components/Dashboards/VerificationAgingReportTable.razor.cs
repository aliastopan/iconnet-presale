using IConnet.Presale.WebApp.Models.Presales;

namespace IConnet.Presale.WebApp.Components.Dashboards;

public partial class VerificationAgingReportTable
{
    [Parameter]
    public List<VerificationAgingReportModel> Models { get; set; } = [];
}
