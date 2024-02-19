using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Services;

public class ChatTemplateService
{
    private readonly List<string> _chatTemplates = new List<string>();
    private readonly List<MarkupString> _htmlChatTemplates = new List<MarkupString>();

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
}
