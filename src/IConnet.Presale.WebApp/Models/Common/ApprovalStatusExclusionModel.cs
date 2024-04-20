namespace IConnet.Presale.WebApp.Models.Common;

public class ApprovalStatusExclusionModel
{
    public HashSet<ApprovalStatus> ApprovalStatus { get; set; } = [];
    public HashSet<ApprovalStatus> Inclusion { get; set; } = [];
    public HashSet<ApprovalStatus> Exclusion => GetApprovalStatusExclusions();

    public ApprovalStatusExclusionModel()
    {
        var approvalStatusEnums = EnumProcessor.GetAllEnumValues<ApprovalStatus>();

        ApprovalStatus = new HashSet<ApprovalStatus>(approvalStatusEnums);
        Inclusion = new HashSet<ApprovalStatus>(approvalStatusEnums);
    }

    private HashSet<ApprovalStatus> GetApprovalStatusExclusions()
    {
        var exclusion = new HashSet<ApprovalStatus>(ApprovalStatus);

        exclusion.ExceptWith(Inclusion);
        return exclusion;
    }
}
