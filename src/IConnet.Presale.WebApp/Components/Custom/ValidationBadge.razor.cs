namespace IConnet.Presale.WebApp.Components.Custom;

public partial class ValidationBadge : ComponentBase
{
    [Parameter]
    public ValidationStatus ValidationStatus { get; set; } = default!;

    public string GetCssBadge()
    {
        switch (ValidationStatus)
        {
            case ValidationStatus.TidakSesuai:
                return "validation-badge-invalid";
            case ValidationStatus.Sesuai:
                return "validation-badge-valid";
            default:
                return "validation-badge-waiting";
        }
    }
}
