namespace IConnet.Presale.WebApp.Components.Custom;

public partial class WhatsAppContact : ComponentBase
{
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;

    [Parameter] public string WhatsApp { get; set; } = string.Empty;

    protected async Task OnContactAsync()
    {
        LogSwitch.Debug("WhatsApp: {contact}", WhatsApp);
        var url = $"http://{WhatsApp}";

        await JsRuntime.InvokeVoidAsync("open", url, "_blank");
    }
}
