namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class ChatTemplateDialog : IDialogContentComponent<WorkPaper>
{
    [Inject] public ChatTemplateService ChatTemplateService { get; set; } = default!;
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        ChatTemplateService.ActiveWorkPaper = Content;
        // LogSwitch.Debug("WorkPaper {0}", Content.ApprovalOpportunity.IdPermohonan);
    }
}
