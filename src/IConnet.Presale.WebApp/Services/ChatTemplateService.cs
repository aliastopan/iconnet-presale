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
}
