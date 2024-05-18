namespace IConnet.Presale.WebApp.Services;

public class ChatTemplateEditService
{
    public List<ChatTemplateSettingModel> AddStash { get; set; } = [];
    public List<ChatTemplateSettingModel> EditStash { get; set; } = [];

    public void ResetStash()
    {
        AddStash.Clear();
        EditStash.Clear();
    }
}
