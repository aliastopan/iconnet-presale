using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Contracts.ChatTemplate;
using IConnet.Presale.Shared.Interfaces.Models.ChatTemplate;

namespace IConnet.Presale.Application.ChatTemplates.Queries;

public class GetChatTemplateQuery(string templateName) : IGetChatTemplateModel,
    IRequest<Result<GetChatTemplateResponse>>
{
    [Required]
    public string TemplateName { get; } = templateName;
}
