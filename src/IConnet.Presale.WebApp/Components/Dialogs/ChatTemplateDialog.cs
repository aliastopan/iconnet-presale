namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class ChatTemplateDialog : IDialogContentComponent<WorkPaper>
{
    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;
}
