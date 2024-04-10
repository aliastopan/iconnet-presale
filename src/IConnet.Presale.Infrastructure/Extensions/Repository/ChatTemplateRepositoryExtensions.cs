using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class ChatTemplateRepositoryExtensions
{
    public static List<ChatTemplate> GetChatTemplates(this AppDbContext context, string templateName)
    {
        return context.ChatTemplates.Where(x => x.TemplateName == templateName)
            .OrderBy(x => x.Sequence)
            .ToList();
    }

    public static List<ChatTemplate> GetAllChatTemplates(this AppDbContext context)
    {
        return context.ChatTemplates.ToList();
    }
}
