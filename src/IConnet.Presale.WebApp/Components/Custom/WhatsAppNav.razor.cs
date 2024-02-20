namespace IConnet.Presale.WebApp.Components.Custom;

public partial class WhatsAppNav : ComponentBase
{
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;

    [Parameter] public string Contact { get; set; } = string.Empty;

    protected async Task OnContactAsync()
    {
        LogSwitch.Debug("WhatsApp: {contact}", Contact);
        var url = $"http://{Contact}";

        await JsRuntime.InvokeVoidAsync("open", url, "_blank");
    }
}