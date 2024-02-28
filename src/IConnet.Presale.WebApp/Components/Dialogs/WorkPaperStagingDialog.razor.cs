namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class WorkPaperStagingDialog : IDialogContentComponent<WorkPaper>
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    private async Task SaveAsync()
    {
        await StageWorkloadAsync();
        await Dialog.CloseAsync(Content);
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    private async Task StageWorkloadAsync()
    {
        Content.WorkPaperLevel = WorkPaperLevel.Validating;
        Content.Shift = (await SessionService.GetJobShiftAsync()).ToString();
        Content.SignatureHelpdeskInCharge = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };
    }
}
