using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.Application.ChatTemplates.Queries.GetChatTemplates;

public class GetChatTemplatesQuery(string templateName) : IRequest<Result<GetChatTemplatesQueryResponse>>
{
    [Required]
    public string TemplateName { get; } = templateName;
}
