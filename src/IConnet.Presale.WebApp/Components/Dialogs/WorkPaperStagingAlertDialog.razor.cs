namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class WorkPaperStagingAlertDialog : IDialogContentComponent<WorkPaper>
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    private async Task SaveAsync()
    {
        await RestageWorkloadAsync();
        await Dialog.CloseAsync(Content);
    }

    private async Task DeleteAsync()
    {
        UnstageWorkload();
        await Dialog.CloseAsync(Content);
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    private async Task RestageWorkloadAsync()
    {
        // Log.Warning("Re-staging");
        Content.Shift = (await SessionService.GetJobShiftAsync()).ToString();
        Content.HelpdeskInCharge = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };
    }

    private void UnstageWorkload()
    {
        // Log.Warning("Un-staging");

        Content.HelpdeskInCharge = new ActionSignature
        {
            AccountIdSignature = Guid.Empty,
            Alias = string.Empty,
            TglAksi = DateTimeService.Zero
        };
    }
}