using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class ChatTemplateManager : IChatTemplateManager
{
    private readonly AppDbContextFactory _dbContextFactory;

    public ChatTemplateManager(AppDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public Result<ICollection<ChatTemplate>> TryGetChatTemplates(string templateName)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var chatTemplates = dbContext.GetChatTemplates(templateName);
        if (chatTemplates is null)
        {
            var error = new Error($"Template '{templateName}' not found.", ErrorSeverity.Warning);
            return Result<ICollection<ChatTemplate>>.NotFound(error);
        }

        return Result<ICollection<ChatTemplate>>.Ok(chatTemplates);
    }
}
