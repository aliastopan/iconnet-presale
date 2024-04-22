using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Tabulations;

public partial class ImportSlaTabulation
{
    [Inject] protected SlaService SlaService { get; set; } = default!;

    [Parameter]
    public List<ImportAgingReportModel> Models { get; set; } = [];

    [Parameter]
    public bool IsPageView { get; set; }

    protected string GetSlaVerdict(ImportAgingReportModel report)
    {
        if (report.ImportTotal == 0)
        {
            return "Kosong";
        }

        return SlaService.GetSlaImportVerdict(report.PacId, Models);
    }
}
