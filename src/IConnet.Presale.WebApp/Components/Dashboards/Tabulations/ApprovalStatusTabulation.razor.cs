using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Tabulations;

public partial class ApprovalStatusTabulation : ComponentBase
{
    [Parameter]
    public List<ApprovalStatusReportModel> Models { get; set; } = [];

    protected ApexChartOptions<ApprovalStatusReportModel> Options { get; set; } = default!;

    protected double GetSum(string key)
    {
        return Models.Sum(x => (double)x.StatusPerOffice[key]);
    }

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<ApprovalStatusReportModel>
        {
            PlotOptions = new PlotOptions
            {
                Bar = new PlotOptionsBar
                {
                    Horizontal = true
                }
            },
            Chart = new Chart
            {
                Toolbar = new Toolbar { Show = false }
            },
            Colors = new List<string> { "#0e700e", "#ff0033", "#1e6bc9", "#ff7300", "#0e7067" }
        };

        // Options.Chart.Toolbar.Show = false;
    }
}
