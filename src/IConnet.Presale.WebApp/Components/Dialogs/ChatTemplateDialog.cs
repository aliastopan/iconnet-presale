namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class ChatTemplateDialog : ComponentBase, IDialogContentComponent<WorkPaper>
{
    [Inject] public ChatTemplateService ChatTemplateService { get; set; } = default!;
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var elementId = "chat-template";
            await JsRuntime.InvokeVoidAsync("scrollToElement", elementId);

            LogSwitch.Debug("auto-scroll");
        }
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }
}
