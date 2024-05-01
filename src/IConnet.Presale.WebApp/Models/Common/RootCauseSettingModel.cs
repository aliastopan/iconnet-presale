using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Models.Common;

public class RootCauseSettingModel
{
    public RootCauseSettingModel(Guid rootCauseId, int order, string cause, string classification,
        bool isDeleted, bool isOnVerification)
    {
        RootCauseId = rootCauseId;
        Order = order;
        Cause = cause;
        Classification = classification;
        IsDeleted = isDeleted;

        IsEnabled = !isDeleted;
        IsOnVerification = isOnVerification;
    }

    public RootCauseSettingModel(RootCausesDto rootCausesDto)
    {
        RootCauseId = rootCausesDto.RootCauseId;
        Order = rootCausesDto.Order;
        Cause = rootCausesDto.Cause;
        Classification = rootCausesDto.Classification;
        IsDeleted = rootCausesDto.IsDeleted;
        IsOnVerification = rootCausesDto.IsOnVerification;

        IsEnabled = !rootCausesDto.IsDeleted;
        IsIncluded = rootCausesDto.IsOnVerification;
    }

    public Guid RootCauseId { get; init; }
    public int Order { get; init; }
    public string Cause { get; init; }
    public string Classification { get; init; }
    public bool IsDeleted { get; init; }
    public bool IsOnVerification { get; init; }

    public bool IsEnabled { get; set; }
    public bool IsToggledSoftDeletion => IsDeleted == IsEnabled;
    public bool SoftDeletionToggleValue => !IsEnabled;

    public bool IsIncluded { get; set; }
    public bool IsToggledOnVerification => IsOnVerification != IsIncluded;
    public bool OnVerificationToggleValue => IsIncluded;

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

    public void OnIsIncludedChanged(bool isIncluded)
    {
        IsIncluded = isIncluded;
    }
}
