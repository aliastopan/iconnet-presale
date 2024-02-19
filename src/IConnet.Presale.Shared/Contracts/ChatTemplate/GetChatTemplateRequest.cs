using IConnet.Presale.Shared.Interfaces.Models.ChatTemplate;

namespace IConnet.Presale.Shared.Contracts.ChatTemplate;

public record GetChatTemplateRequest(string TemplateName) : IGetChatTemplateModel;
