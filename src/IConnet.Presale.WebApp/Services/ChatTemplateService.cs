using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Services;

public class ChatTemplateService
{
    private readonly List<string> _chatTemplates = new List<string>();

    public IReadOnlyCollection<string> ChatTemplates => _chatTemplates.AsReadOnly();

    public void InitializeChatTemplate(ICollection<ChatTemplateDto> chatTemplateDtos)
    {
        foreach (var chatTemplate in chatTemplateDtos)
        {
            _chatTemplates.Add(chatTemplate.Content);
        }

        LogSwitch.Debug("Chat template has been initialized ({length} sequences)", _chatTemplates.Count);
    }
}
