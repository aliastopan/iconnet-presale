using IConnet.Presale.Shared.Common;

namespace IConnet.Presale.WebApp.Components.Custom;

public partial class ChatBubble : ComponentBase
{
    [Parameter] public WorkPaper? ActiveWorkPaper { get; set; }
    [Parameter] public ChatTemplateModel ChatTemplate { get; set; } = default!;
    [Parameter] public string Sender { get; set; } = string.Empty;
    [Parameter] public string Timestamp { get; set; } = string.Empty;

    public MarkupString GetChatTemplate()
    {
        if (ActiveWorkPaper is null)
        {
            return ChatTemplate.HtmlContent;
        }

        return ChatTemplate.HtmlContent
            .ReplacePlaceholder(PlaceholderText.IdPln, ActiveWorkPaper.ApprovalOpportunity.Pemohon.IdPln)
            .ReplacePlaceholder(PlaceholderText.NamaPelanggan, ActiveWorkPaper.ApprovalOpportunity.Pemohon.NamaLengkap)
            .ReplacePlaceholder(PlaceholderText.NomorTelepon, ActiveWorkPaper.ApprovalOpportunity.Pemohon.NomorTelepon)
            .ReplacePlaceholder(PlaceholderText.AlamatEmail, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Email)
            .ReplacePlaceholder(PlaceholderText.AlamatPemasangan, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Alamat)
            .ReplacePlaceholder(PlaceholderText.Nik, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Nik)
            .ReplacePlaceholder(PlaceholderText.Npwp, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Npwp);
    }

    public void CopyChatTemplate()
    {
        LogSwitch.Debug("Copying Chat Template with sequence {index}", ChatTemplate.Sequence);
    }
}
