using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Handlers;

internal sealed class ChatTemplateHandler : IChatTemplateHandler
{
    private readonly AppDbContextFactory _dbContextFactory;

    public ChatTemplateHandler(AppDbContextFactory dbContextFactory)
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

    public Result<ICollection<ChatTemplate>> TryGetAvailableChatTemplates()
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var chatTemplates = dbContext.GetAvailableChatTemplates();
        if (chatTemplates.Count == 0)
        {
            var error = new Error($"No Chat Template were found.", ErrorSeverity.Warning);
            return Result<ICollection<ChatTemplate>>.NotFound(error);
        }

        return Result<ICollection<ChatTemplate>>.Ok(chatTemplates);
    }
}
