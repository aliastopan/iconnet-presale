using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class ChatTemplateRepositoryExtensions
{
    public static ChatTemplate? FindChatTemplate(this AppDbContext context, Guid ChatTemplateId)
    {
        return context.ChatTemplates.FirstOrDefault(x => x.ChatTemplateId == ChatTemplateId);
    }

    public static List<ChatTemplate> GetChatTemplates(this AppDbContext context, string templateName)
    {
        return context.ChatTemplates.Where(x => x.TemplateName == templateName)
            .OrderBy(x => x.Sequence)
            .ToList();
    }

    public static List<ChatTemplate> GetAvailableChatTemplates(this AppDbContext context)
    {
        return context.ChatTemplates.ToList();
    }

    public static bool IsChatTemplateNameExist(this AppDbContext context, string templateName)
    {
        return context.ChatTemplates.Any(x => x.TemplateName == templateName);
    }
}
