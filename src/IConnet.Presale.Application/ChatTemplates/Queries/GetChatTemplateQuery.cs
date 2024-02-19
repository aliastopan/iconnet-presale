using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Application.ChatTemplates.Queries;

public class GetChatTemplateQuery(string templateName) : IRequest<Result<GetChatTemplateResponse>>
{
    [Required]
    public string TemplateName { get; } = templateName;
}
