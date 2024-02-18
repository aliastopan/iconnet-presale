namespace IConnet.Presale.WebApp.Components.Custom;

public partial class WhatsAppContact : ComponentBase
{
    [Parameter] public string WhatsApp { get; set; } = string.Empty;

    protected void OnContact()
    {
        LogSwitch.Debug("WhatsApp: {contact}", WhatsApp);
    }
}
