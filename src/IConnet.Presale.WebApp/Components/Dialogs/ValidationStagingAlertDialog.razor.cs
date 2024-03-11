namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class ValidationStagingAlertDialog : IDialogContentComponent<WorkPaper>
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    private async Task SaveAsync()
    {
        await RestageWorkPaperAsync();
        await Dialog.CloseAsync(Content);
    }

    private async Task DeleteAsync()
    {
        UnstageWorkPaper();
        await Dialog.CloseAsync(Content);
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    private async Task RestageWorkPaperAsync()
    {
        Content.Shift = "";
        Content.SetHelpdeskInCharge(new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        });
    }

    private void UnstageWorkPaper()
    {
        Content.SetHelpdeskInCharge(new ActionSignature
        {
            AccountIdSignature = Guid.Empty,
            Alias = string.Empty,
            TglAksi = DateTimeService.Zero
        });
    }
}