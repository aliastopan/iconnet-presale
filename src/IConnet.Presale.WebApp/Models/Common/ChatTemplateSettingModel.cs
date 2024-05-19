using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Models.Common;

public class ChatTemplateSettingModel
{
    public ChatTemplateSettingModel(ChatTemplateDto chatTemplateDto)
    {
        ChatTemplateId = chatTemplateDto.ChatTemplateId;
        TemplateName = chatTemplateDto.TemplateName;
        Sequence = chatTemplateDto.Sequence;
        Content = chatTemplateDto.Content;
        ActionSetting = ChatTemplateAction.Default;
    }

    public ChatTemplateSettingModel(string templateName, int sequence, string content, ChatTemplateAction action)
    {
        ChatTemplateId = Guid.NewGuid();
        TemplateName = templateName;
        Sequence = sequence;
        Content = content;
        ActionSetting = action;
    }

    public ChatTemplateSettingModel(Guid chatTemplateId, string templateName, int sequence, string content, ChatTemplateAction action)
    {
        ChatTemplateId = chatTemplateId;
        TemplateName = templateName;
        Sequence = sequence;
        Content = content;
        ActionSetting = action;
    }

    public Guid ChatTemplateId { get; set; }
    public string TemplateName { get; set; }
    public int Sequence { get; set; }
    public string Content { get; set; }
    public ChatTemplateAction ActionSetting { get; set; }

    public ChatTemplateSettingModel Copy()
    {
        return new ChatTemplateSettingModel(
            this.ChatTemplateId,
            this.TemplateName,
            this.Sequence,
            this.Content,
            this.ActionSetting);
    }
}