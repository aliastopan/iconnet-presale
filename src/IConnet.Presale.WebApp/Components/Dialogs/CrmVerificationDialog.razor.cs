namespace IConnet.Presale.WebApp.Components.Dialogs;

public partial class CrmVerificationDialog : IDialogContentComponent<WorkPaper>
{
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter]
    public WorkPaper Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected async Task SaveAsync()
    {
        await VerifyCrmAsync();
        await Dialog.CloseAsync(Content);
    }

    protected async Task DeleteAsync()
    {
        await DeleteCrmAsync();
        await Dialog.CloseAsync(Content);
    }

    protected async Task ArchiveAsync()
    {
        await ArchiveCrmAsync();
        await Dialog.CloseAsync(Content);
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    private async Task VerifyCrmAsync()
    {
        Content.WorkPaperLevel = WorkPaperLevel.ImportVerified;
        Content.ApprovalOpportunity.StatusImport = ImportStatus.Verified;
        Content.ApprovalOpportunity.SignatureVerifikasiImport = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };
    }

    private async Task DeleteCrmAsync()
    {
        Content.WorkPaperLevel = WorkPaperLevel.ImportInvalid;
        Content.ApprovalOpportunity.StatusImport = ImportStatus.Invalid;
        Content.ApprovalOpportunity.SignatureVerifikasiImport = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };
    }

    private async Task ArchiveCrmAsync()
    {
        Content.WorkPaperLevel = WorkPaperLevel.ImportArchived;
        Content.ApprovalOpportunity.StatusImport = ImportStatus.Invalid;
        Content.ApprovalOpportunity.SignatureVerifikasiImport = new ActionSignature
        {
            AccountIdSignature = await SessionService.GetUserAccountIdAsync(),
            Alias = await SessionService.GetSessionAliasAsync(),
            TglAksi = DateTimeService.DateTimeOffsetNow.DateTime
        };
    }

    protected async Task OnOpenGoogleMapAsync()
    {
        var url = Content.ApprovalOpportunity.Regional.Koordinat.GetGoogleMapLink();

        await JsRuntime.InvokeVoidAsync("open", url, "_blank");
    }
}
