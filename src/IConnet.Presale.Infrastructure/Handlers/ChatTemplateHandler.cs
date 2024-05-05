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

    public async Task<Result> TryAddChatTemplate(Guid chatTemplateId, string templateName, int sequence, string content)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        bool isTemplateNameExist = dbContext.IsChatTemplateNameExist(templateName);

        if(isTemplateNameExist)
        {
            var error = new Error($"Template name already exist.", ErrorSeverity.Warning);
            return Result.Conflict(error);
        }

        var chatTemplate = new ChatTemplate
        {
            ChatTemplateId = chatTemplateId,
            TemplateName = templateName,
            Sequence = sequence,
            Content = content
        };

        dbContext.ChatTemplates.Add(chatTemplate);

        await dbContext.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> TryEditChatTemplate(Guid chatTemplateId, string templateName, int sequence, string content, bool isDeleted)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var chatTemplates = dbContext.GetChatTemplates(templateName);
        if (chatTemplates is null)
        {
            var error = new Error($"Template '{templateName}' not found.", ErrorSeverity.Warning);
            return Result.NotFound(error);
        }

        var target = chatTemplates.FirstOrDefault(x => x.ChatTemplateId == chatTemplateId);
        if (target is null)
        {
            var error = new Error($"Chat Template with ID '{chatTemplateId}' not found.", ErrorSeverity.Warning);
            return Result.NotFound(error);
        }

        if (isDeleted)
        {
            dbContext.ChatTemplates.Remove(target);

            return Result.Ok();
        }
        else
        {
            target.Sequence = sequence;
            target.Content = content;

            dbContext.ChatTemplates.Update(target);

            await dbContext.SaveChangesAsync();

            return Result.Ok();
        }
    }
}
