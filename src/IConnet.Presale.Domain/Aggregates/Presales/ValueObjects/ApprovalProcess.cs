#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ApprovalProcess : ValueObject
{
    public ApprovalProcess()
    {

    }

    public ApprovalProcess(ApprovalStatus statusApproval, string rootCause, string keterangan,
        string jarakShareLoc, string jarakICrmPlus, DateTime vaTerbit)
    {
        StatusApproval = statusApproval;
        RootCause = rootCause;
        Keterangan = keterangan;
        JarakShareLoc = jarakShareLoc;
        JarakICrmPlus = jarakICrmPlus;
        VaTerbit = vaTerbit;
    }

    public ActionSignature SignatureApproval { get; init;} = new();
    public ApprovalStatus StatusApproval { get; init; } = default;
    public string RootCause { get; init; } = string.Empty;
    public string Keterangan { get; init; } = string.Empty;
    public string JarakShareLoc { get; init; } = string.Empty;
    public string JarakICrmPlus { get; init; } = string.Empty;
    public DateTime VaTerbit { get; init; } = default;

    [NotMapped]
    public bool IsOnGoing => SignatureApproval.IsEmptySignature();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SignatureApproval;
        yield return StatusApproval;
        yield return RootCause;
        yield return Keterangan;
        yield return JarakShareLoc;
        yield return JarakICrmPlus;
        yield return VaTerbit;
    }
}
