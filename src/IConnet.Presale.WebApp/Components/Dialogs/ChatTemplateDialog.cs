namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class ChatTemplateDialog : IDialogContentComponent<WorkPaper>
{
    [Inject] public ChatTemplateService ChatTemplateService { get; set; } = default!;

    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;
}
