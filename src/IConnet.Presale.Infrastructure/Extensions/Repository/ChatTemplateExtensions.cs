using Microsoft.EntityFrameworkCore;
using IConnet.Presale.Domain.Entities;

namespace IConnet.Presale.Infrastructure.Extensions.Repository;

internal static class ChatTemplateExtensions
{
    public static async Task<List<ChatTemplate>> GetChatTemplateAsync(this AppDbContext context, string templateName)
    {
        return await context.ChatTemplates.Where(x => x.TemplateName == templateName).ToListAsync();
    }
}
