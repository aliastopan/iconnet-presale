namespace IConnet.Presale.WebApp.Components.Custom;

public partial class WhatsAppNav : ComponentBase
{
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;

    [Parameter] public string Contact { get; set; } = string.Empty;
    [Parameter] public EventCallback ChatCallMulai { get; set; }

    protected async Task OnContactAsync()
    {
        var url = $"http://{Contact}";

        await JsRuntime.InvokeVoidAsync("open", url, "_blank");
        await ChatCallMulai.InvokeAsync();
    }
}
