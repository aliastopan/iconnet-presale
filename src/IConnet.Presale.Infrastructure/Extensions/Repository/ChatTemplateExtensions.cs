using Microsoft.EntityFrameworkCore;
using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class ChatTemplateExtensions
{
    public static List<ChatTemplate> GetChatTemplates(this AppDbContext context, string templateName)
    {
        return context.ChatTemplates.Where(x => x.TemplateName == templateName)
            .OrderBy(x => x.Sequence)
            .ToList();
    }
}
