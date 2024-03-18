namespace IConnet.Presale.WebApp.Components.Custom.Badges;

public partial class ValidationBadge : ComponentBase
{
    [Parameter]
    public ValidationStatus ValidationStatus { get; set; } = default!;

    [Parameter]
    public bool IncludeNotResponding { get; set; } = false;

    [Parameter]
    public bool WidthConstraint { get; set; } = false;

    protected string GetCssBadge()
    {
        switch (ValidationStatus)
        {
            case ValidationStatus.MenungguValidasi:
                if(IncludeNotResponding)
                    return "validation-badge-not-responding";
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
            && IncludeNotResponding)
        {
            return "TIDAK ADA RESPONS";
        }

        return EnumProcessor.EnumToDisplayString(ValidationStatus);
    }

}
