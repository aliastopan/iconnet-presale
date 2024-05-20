namespace IConnet.Presale.Shared.Interfaces.Models.Common;

public interface IEditChatTemplate
{
    Guid ChatTemplateId { get; }
    string TemplateName { get; }
    int Sequence { get; }
    string Content { get; }
    bool IsDeleted { get; }
}
