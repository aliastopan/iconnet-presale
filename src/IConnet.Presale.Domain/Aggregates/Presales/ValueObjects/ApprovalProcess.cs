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

    public ApprovalProcess WithSignatureApproval(ActionSignature signatureApproval)
    {
        return new ApprovalProcess
        {
            SignatureApproval = signatureApproval,
            StatusApproval = this.StatusApproval,
            RootCause = this.RootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithSignatureApproval(ApprovalStatus approvalStatus)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = approvalStatus,
            RootCause = this.RootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithRootCause(string rootCause)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = this.StatusApproval,
            RootCause = rootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithKeterangan(string keterangan)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = this.StatusApproval,
            RootCause = this.RootCause,
            Keterangan = keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithJarakShareLoc(string jarakShareLoc)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = this.StatusApproval,
            RootCause = this.RootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = jarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithJarakICrmPlus(string jarakShareLoc)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = this.StatusApproval,
            RootCause = this.RootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = jarakShareLoc,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithVaTerbit(DateTime vaTerbit)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = this.StatusApproval,
            RootCause = this.RootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            VaTerbit = vaTerbit
        };
    }

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
