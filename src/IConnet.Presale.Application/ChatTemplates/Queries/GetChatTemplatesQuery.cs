using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Application.ChatTemplates.Queries;

public class GetChatTemplatesQuery(string templateName) : IRequest<Result<GetChatTemplatesResponse>>
{
    [Required]
    public string TemplateName { get; } = templateName;
}
