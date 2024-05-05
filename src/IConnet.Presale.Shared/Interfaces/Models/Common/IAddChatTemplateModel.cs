namespace IConnet.Presale.Shared.Interfaces.Models.Common;

public interface IAddChatTemplateModel
{
    Guid ChatTemplateId { get; }
    string TemplateName { get; }
    int Sequence { get; }
    string Content { get; }
}
