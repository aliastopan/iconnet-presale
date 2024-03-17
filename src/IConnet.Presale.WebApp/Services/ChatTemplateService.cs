namespace IConnet.Presale.WebApp.Services;

public class ChatTemplateService
{
    private readonly ChatTemplateManager _chatTemplateManager;

    public ChatTemplateService(ChatTemplateManager chatTemplateManager)
    {
        _chatTemplateManager = chatTemplateManager;
    }

    public IReadOnlyCollection<ChatTemplateModel> ChatTemplateModels => _chatTemplateManager.ChatTemplateModels.AsReadOnly();
}
