using System.ComponentModel.DataAnnotations;
using IConnet.Presale.Shared.Interfaces.Models.Common;

namespace IConnet.Presale.Application.ChatTemplates.Commands.ChatTemplateAction;

public class ChatTemplateActionCommand : IChatTemplateActionModel, IRequest<Result>
{
    public ChatTemplateActionCommand(Guid chatTemplateId, string templateName,
        int sequence, string content, int action)
    {
        ChatTemplateId = chatTemplateId;
        TemplateName = templateName;
        Sequence = sequence;
        Content = content;
        Action = action;
    }

    [Required] public Guid ChatTemplateId { get; set; }
    [Required] public string TemplateName { get; set; }
    [Required] public int Sequence { get; set; }
    [Required] public string Content { get; set; }
    [Required] public int Action { get; set; }
}
