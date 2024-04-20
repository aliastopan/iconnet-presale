using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Models.Common;

public class RootCauseSettingModel
{
    public RootCauseSettingModel(Guid rootCauseId, int order, string cause, bool isDeleted)
    {
        RootCauseId = rootCauseId;
        Order = order;
        Cause = cause;
        IsDeleted = isDeleted;

        IsEnabled = !isDeleted;
    }

    public RootCauseSettingModel(RootCausesDto rootCausesDto)
    {
        RootCauseId = rootCausesDto.RootCauseId;
        Order = rootCausesDto.Order;
        Cause = rootCausesDto.Cause;
        IsDeleted = rootCausesDto.IsDeleted;

        IsEnabled = !rootCausesDto.IsDeleted;
    }

    public Guid RootCauseId { get; init; }
    public int Order { get; init; }
    public string Cause { get; init; }
    public bool IsDeleted { get; init; }

    public bool IsEnabled { get; set; }
    public bool IsToggledSoftDeletion => IsDeleted == IsEnabled;
    public bool SoftDeletionToggleValue => !IsEnabled;

    public string GetStatus()
    {
        return !IsDeleted
            ? "Enabled"
            : "Disabled";
    }

    public void OnIsEnabledChanged(bool isEnabled)
    {
        IsEnabled = isEnabled;
    }
}
