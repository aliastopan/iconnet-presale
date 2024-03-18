using IConnet.Presale.Shared.Common;

namespace IConnet.Presale.WebApp.Components.Custom;

public partial class ChatBubble : ComponentBase
{
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] public IDateTimeService DateTimeService { get; set; } = default!;

    [Parameter] public WorkPaper? ActiveWorkPaper { get; set; }
    [Parameter] public ChatTemplateModel ChatTemplate { get; set; } = default!;
    [Parameter] public string Sender { get; set; } = string.Empty;
    [Parameter] public string Timestamp { get; set; } = string.Empty;

    public bool IsCopy { get; set; } = false;

    public MarkupString GetChatTemplate()
    {
        if (ActiveWorkPaper is null)
        {
            return ChatTemplate.HtmlContent;
        }

        return ChatTemplate.HtmlContent
            .ReplacePlaceholder(PlaceholderText.Waktu, DateTimeService.GetTimeIdentifier().ToLower())
            .ReplacePlaceholder(PlaceholderText.IdPln, ActiveWorkPaper.ApprovalOpportunity.Pemohon.IdPln)
            .ReplacePlaceholder(PlaceholderText.NamaPelanggan, ActiveWorkPaper.ApprovalOpportunity.Pemohon.NamaPelanggan)
            .ReplacePlaceholder(PlaceholderText.NomorTelepon, ActiveWorkPaper.ApprovalOpportunity.Pemohon.NomorTelepon)
            .ReplacePlaceholder(PlaceholderText.AlamatEmail, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Email)
            .ReplacePlaceholder(PlaceholderText.AlamatPemasangan, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Alamat)
            .ReplacePlaceholder(PlaceholderText.Nik, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Nik)
            .ReplacePlaceholder(PlaceholderText.Npwp, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Npwp);
    }

    public async Task CopyChatTemplateAsync()
    {
        if (ActiveWorkPaper is null)
        {
            return;
        }

        LogSwitch.Debug("Copying clipboard chat {sequence}", ChatTemplate.Sequence);

        IsCopy = true;

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", ChatTemplate.Content
            .ReplacePlaceholder(PlaceholderText.Waktu, DateTimeService.GetTimeIdentifier().ToLower())
            .ReplacePlaceholder(PlaceholderText.IdPln, ActiveWorkPaper.ApprovalOpportunity.Pemohon.IdPln)
            .ReplacePlaceholder(PlaceholderText.NamaPelanggan, ActiveWorkPaper.ApprovalOpportunity.Pemohon.NamaPelanggan)
            .ReplacePlaceholder(PlaceholderText.NomorTelepon, ActiveWorkPaper.ApprovalOpportunity.Pemohon.NomorTelepon)
            .ReplacePlaceholder(PlaceholderText.AlamatEmail, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Email)
            .ReplacePlaceholder(PlaceholderText.AlamatPemasangan, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Alamat)
            .ReplacePlaceholder(PlaceholderText.Nik, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Nik)
            .ReplacePlaceholder(PlaceholderText.Npwp, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Npwp));
    }
}
