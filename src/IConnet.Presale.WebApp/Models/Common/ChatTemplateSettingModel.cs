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
        SettingStatus = ChatTemplateSettingStatus.Default;
    }

    public Guid ChatTemplateId { get; set; }
    public string TemplateName { get; set; }
    public int Sequence { get; set; }
    public string Content { get; set; }
    public ChatTemplateSettingStatus SettingStatus { get; set;}
}

public enum ChatTemplateSettingStatus
{
    Default,
    ContentEdit,
    ContentAdd,
}