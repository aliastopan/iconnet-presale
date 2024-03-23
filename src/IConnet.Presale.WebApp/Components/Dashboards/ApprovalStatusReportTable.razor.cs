namespace IConnet.Presale.WebApp.Components.Dashboards;

public partial class ApprovalStatusReportTable : ComponentBase
{
    [Parameter]
    public IQueryable<WorkPaper>? PresaleData { get; set; }
}
