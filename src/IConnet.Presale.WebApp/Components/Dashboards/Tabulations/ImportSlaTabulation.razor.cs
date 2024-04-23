using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Tabulations;

public partial class ImportSlaTabulation
{
    [Inject] protected SlaService SlaService { get; set; } = default!;

    [Parameter]
    public List<ImportAgingReportModel> Models { get; set; } = [];

    [Parameter]
    public bool IsPageView { get; set; }

    protected bool IsWinningSlaImport(ImportAgingReportModel report, out string verdict)
    {
        if (report.ImportTotal == 0)
        {
            verdict = "kosong";
            return false;
        }

        verdict = SlaService.GetSlaImportVerdict(report.PacId, Models);
        return true;
    }
}
