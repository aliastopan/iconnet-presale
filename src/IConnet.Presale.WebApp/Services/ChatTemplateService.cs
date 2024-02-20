using IConnet.Presale.Shared.Common;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Services;

public class ChatTemplateService
{
    private readonly List<ChatTemplateModel> _chatTemplateModels = new List<ChatTemplateModel>();

    public WorkPaper? ActiveWorkPaper { get; set; }
    public IReadOnlyCollection<ChatTemplateModel> ChatTemplateModels => _chatTemplateModels.AsReadOnly();

    public void InitializeChatTemplate(ICollection<ChatTemplateDto> chatTemplateDtos)
    {
        foreach (var chatTemplate in chatTemplateDtos)
        {
            _chatTemplateModels.Add(new ChatTemplateModel
            {
                Sequence = chatTemplate.Sequence,
                Content = chatTemplate.Content,
                HtmlContent = (MarkupString)chatTemplate.Content
                    .FormatHtmlBreak()
                    .FormatHtmlBold()
                    .FormatHtmlItalic()
            });
        }

        LogSwitch.Debug("Chat template has been initialized ({length} sequences)", _chatTemplateModels.Count);
    }

    public MarkupString ReplacePlaceholder(int sequence)
    {
        if (ActiveWorkPaper is null)
        {
            LogSwitch.Debug("WorkPaper is null");
            return _chatTemplateModels.ElementAt(sequence).HtmlContent;
        }

        return _chatTemplateModels.ElementAt(sequence).HtmlContent
            .ReplacePlaceholder(PlaceholderText.IdPln, ActiveWorkPaper.ApprovalOpportunity.Pemohon.IdPln)
            .ReplacePlaceholder(PlaceholderText.NamaPelanggan, ActiveWorkPaper.ApprovalOpportunity.Pemohon.NamaLengkap)
            .ReplacePlaceholder(PlaceholderText.NomorTelepon, ActiveWorkPaper.ApprovalOpportunity.Pemohon.NomorTelepon)
            .ReplacePlaceholder(PlaceholderText.AlamatEmail, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Email)
            .ReplacePlaceholder(PlaceholderText.AlamatPemasangan, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Alamat)
            .ReplacePlaceholder(PlaceholderText.Nik, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Nik)
            .ReplacePlaceholder(PlaceholderText.Npwp, ActiveWorkPaper.ApprovalOpportunity.Pemohon.Npwp);
    }
}
