using ApexCharts;
using IConnet.Presale.WebApp.Models.Presales.Reports;

namespace IConnet.Presale.WebApp.Components.Dashboards.Metrics;

public partial class ApprovalStatusMetric : ComponentBase
{
    [Parameter]
    public List<ApprovalStatusReportModel> Models { get; set; } = [];

    protected List<ApprovalStatusMetricModel> Metrics { get; set; } = [];
    protected ApexChartOptions<ApprovalStatusMetricModel> Options { get; set; } = default!;

    protected override void OnInitialized()
    {
        Options = new ApexChartOptions<ApprovalStatusMetricModel>
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
            Colors = ["#1e6bc9", "#202020", "#ff0033", "#ff7300", "#0e700e"]
        };

        Metrics = ConvertToMetrics(Models);
        PrintFirstMetric();
    }

    public List<ApprovalStatusMetricModel> ConvertToMetrics(List<ApprovalStatusReportModel> models)
    {
        var metrics = models.SelectMany(model => model.StatusPerOffice.Select(status => new { model.ApprovalStatus, Office = status.Key, Count = status.Value }))
                            .GroupBy(x => x.Office)
                            .Select(group => new ApprovalStatusMetricModel
                            {
                                Office = group.Key,
                                Status = group.ToDictionary(x => x.ApprovalStatus, x => x.Count)
                            })
                            .ToList();

        return metrics;
    }

    public void PrintFirstMetric()
    {
        if (Metrics != null && Metrics.Count > 0)
        {
            var firstMetric = Metrics[0];
            Console.WriteLine($"Office: {firstMetric.Office}");
            Console.WriteLine("Status Counts:");
            foreach (var status in firstMetric.Status)
            {
                Console.WriteLine($"{status.Key}: {status.Value}");
            }
        }
        else
        {
            Console.WriteLine("No metrics available.");
        }
    }

    private static List<ApprovalStatusMetricModel> ConvertToApprovalStatusMetric(List<ApprovalStatusReportModel> models)
    {
        var result = new List<ApprovalStatusMetricModel>();

        foreach (var model in models)
        {
            var metric = new ApprovalStatusMetricModel
            {
                Office = string.Empty,
                Status = new Dictionary<ApprovalStatus, int>
                {
                    { model.ApprovalStatus, 0 }
                }
            };

            foreach (var kvp in model.StatusPerOffice)
            {
                var office = kvp.Key;
                var approvalStatus = model.ApprovalStatus;
                var count = kvp.Value;

                if (metric.Office == string.Empty)
                {
                    metric.Office = office;
                }
                else if (metric.Office != office)
                {
                    result.Add(metric);
                    metric = new ApprovalStatusMetricModel
                    {
                        Office = office,
                        Status = new Dictionary<ApprovalStatus, int>
                        {
                            { approvalStatus, count }
                        }
                    };
                }

                metric.Status[approvalStatus] = metric.Status.GetValueOrDefault(approvalStatus, 0) + count;
            }

            if (!result.Contains(metric))
            {
                result.Add(metric);
            }
        }

        return result;
    }
}
