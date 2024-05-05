namespace IConnet.Presale.Shared.Interfaces.Models.Common;

public interface IChatTemplateActionModel
{
    Guid ChatTemplateId { get; }
    string TemplateName { get; }
    int Sequence { get; }
    string Content { get; }

    int Action { get; }
}
