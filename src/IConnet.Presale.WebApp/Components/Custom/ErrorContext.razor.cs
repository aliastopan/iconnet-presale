namespace IConnet.Presale.WebApp.Components.Custom;

public partial class ErrorContext : ComponentBase
{
    [Parameter] public Exception Exception { get; set; } = default!;
}
