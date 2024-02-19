using IConnet.Presale.Shared.Common;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Services;

public class ChatTemplateService
{
    private readonly List<string> _chatTemplates = new List<string>();
    private readonly List<MarkupString> _htmlChatTemplates = new List<MarkupString>();

    public WorkPaper? ActiveWorkPaper { get; set; }
    public IReadOnlyCollection<MarkupString> HtmlChatTemplates => _htmlChatTemplates.AsReadOnly();

    public void InitializeChatTemplate(ICollection<ChatTemplateDto> chatTemplateDtos)
    {
        foreach (var chatTemplate in chatTemplateDtos)
        {
            _chatTemplates.Add(chatTemplate.Content);
            _htmlChatTemplates.Add((MarkupString)chatTemplate.Content
                .FormatHtmlBreak()
                .FormatHtmlBold()
                .FormatHtmlItalic());
        }

        LogSwitch.Debug("Chat template has been initialized ({length} sequences)", _chatTemplates.Count);
    }

    public MarkupString ReplacePlaceholder(MarkupString chatTemplate)
    {
       if (ActiveWorkPaper is null)
        {
            LogSwitch.Debug("WorkPaper is null");
            return chatTemplate;
        }

        return chatTemplate
            .ReplacePlaceholder(PlaceholderText.IdPln, ActiveWorkPaper.ApprovalOpportunity.Pemohon.IdPln)
            .ReplacePlaceholder(PlaceholderText.NamaPelanggan, ActiveWorkPaper.ApprovalOpportunity.Pemohon.NamaLengkap)
            .ReplacePlaceholder(PlaceholderText.NomorTelepon, ActiveWorkPaper.ApprovalOpportunity.Pemohon.NomorTelepon)
            .ReplacePlaceholder(PlaceholderText.AlamatEmail, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Email)
            .ReplacePlaceholder(PlaceholderText.AlamatPemasangan, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Alamat)
            .ReplacePlaceholder(PlaceholderText.Nik, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Nik)
            .ReplacePlaceholder(PlaceholderText.Npwp, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Npwp);
    }
}
