namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class ApprovalStagingDialog : IDialogContentComponent<WorkPaper>
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    private async Task SaveAsync()
    {
        await StageWorkPaperAsync();
        await Dialog.CloseAsync(Content);
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    protected async Task StageWorkPaperAsync()
    {
        Content.WorkPaperLevel = WorkPaperLevel.WaitingApproval;
        Content.Shift = "";
        Content.SetPlanningAssetCoverageInCharge(new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        });
    }
}
