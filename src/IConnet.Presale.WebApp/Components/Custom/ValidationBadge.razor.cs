namespace IConnet.Presale.WebApp.Components.Custom;

public partial class ValidationBadge : ComponentBase
{
    [Parameter]
    public ValidationStatus ValidationStatus { get; set; } = default!;

    [Parameter]
    public bool IncludeClosedLost { get; set; } = false;

    protected string GetCssBadge()
    {
        switch (ValidationStatus)
        {
            case ValidationStatus.MenungguValidasi:
                if(IncludeClosedLost)
                    return "validation-badge-closed-lost";
                else
                    return "validation-badge-waiting";
            case ValidationStatus.TidakSesuai:
                return "validation-badge-invalid";
            case ValidationStatus.Sesuai:
                return "validation-badge-valid";
            default:
                return "validation-badge-neutral";
        }
    }

    protected string GetValidationStatusString()
    {
        if (ValidationStatus == ValidationStatus.MenungguValidasi
            && IncludeClosedLost)
        {
            return "Closed Lost";
        }

        return EnumProcessor.EnumToDisplayString(ValidationStatus);
    }

}
