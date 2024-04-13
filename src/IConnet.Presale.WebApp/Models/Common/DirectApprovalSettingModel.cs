using IConnet.Presale.Shared.Contracts.Common;

namespace IConnet.Presale.WebApp.Models.Common;

public class DirectApprovalSettingModel
{
    public DirectApprovalSettingModel(Guid directApprovalId, int order, string description, bool isDeleted)
    {
        DirectApprovalId = directApprovalId;
        Order = order;
        Description = description;
        IsDeleted = isDeleted;

        IsEnabled = !isDeleted;
    }

    public DirectApprovalSettingModel(DirectApprovalDto directApprovalDto)
    {
        DirectApprovalId = directApprovalDto.DirectApprovalId;
        Order = directApprovalDto.Order;
        Description = directApprovalDto.Description;
        IsDeleted = directApprovalDto.IsDeleted;

        IsEnabled = !directApprovalDto.IsDeleted;
    }

    public Guid DirectApprovalId { get; init; }
    public int Order { get; init; }
    public string Description { get; init; }
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
