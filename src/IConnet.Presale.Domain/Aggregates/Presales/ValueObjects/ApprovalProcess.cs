#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ApprovalProcess : ValueObject
{
    public ApprovalProcess()
    {
        SignatureApproval = ActionSignature.Empty();
        StatusApproval = ApprovalStatus.InProgress;
        DirectApproval = string.Empty;
        RootCause = string.Empty;
        Keterangan = string.Empty;
        JarakShareLoc = 0;
        JarakICrmPlus = 0;
        SplitterGanti = string.Empty;
        VaTerbit = DateTime.MinValue;
    }

    public ApprovalProcess(ApprovalStatus statusApproval, string directApproval, string rootCause, string keterangan,
        int jarakShareLoc, int jarakICrmPlus, string splitterGanti, DateTime vaTerbit)
    {
        StatusApproval = statusApproval;
        DirectApproval = directApproval;
        RootCause = rootCause;
        Keterangan = keterangan;
        JarakShareLoc = jarakShareLoc;
        JarakICrmPlus = jarakICrmPlus;
        SplitterGanti = splitterGanti;
        VaTerbit = vaTerbit;
    }

    public ActionSignature SignatureApproval { get; init;} = new();
    public ApprovalStatus StatusApproval { get; init; } = default;
    public string DirectApproval { get; init; } = string.Empty;
    public string RootCause { get; init; } = string.Empty;
    public string Keterangan { get; init; } = string.Empty;
    public int JarakShareLoc { get; init; }
    public int JarakICrmPlus { get; init; }
    public string SplitterGanti { get ; init; } = string.Empty;
    public DateTime VaTerbit { get; init; } = default;

    public bool IsOnGoing()
    {
        bool isSignatureEmpty = SignatureApproval.IsEmptySignature();
        bool isPendingOnExpansion = StatusApproval == ApprovalStatus.Expansion;

        return isSignatureEmpty || (!isSignatureEmpty && isPendingOnExpansion);
    }

    public bool IsSplitterGanti()
    {
        return !string.IsNullOrWhiteSpace(SplitterGanti);
    }

    public bool IsDirectlyApproved()
    {
        return StatusApproval == ApprovalStatus.Approve
            && !string.IsNullOrWhiteSpace(DirectApproval);
    }

    public ApprovalProcess WithSignatureApproval(ActionSignature signatureApproval)
    {
        return new ApprovalProcess
        {
            SignatureApproval = signatureApproval,
            StatusApproval = this.StatusApproval,
            DirectApproval = this.DirectApproval,
            RootCause = this.RootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            SplitterGanti = this.SplitterGanti,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithStatusApproval(ApprovalStatus approvalStatus)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = approvalStatus,
            DirectApproval = this.DirectApproval,
            RootCause = this.RootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            SplitterGanti = this.SplitterGanti,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithDirectApproval(string directApproval)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = this.StatusApproval,
            DirectApproval = directApproval,
            RootCause = this.RootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            SplitterGanti = this.SplitterGanti,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithRootCause(string rootCause)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = this.StatusApproval,
            DirectApproval = this.DirectApproval,
            RootCause = rootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            SplitterGanti = this.SplitterGanti,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithKeterangan(string keterangan)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = this.StatusApproval,
            DirectApproval = this.DirectApproval,
            RootCause = this.RootCause,
            Keterangan = keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            SplitterGanti = this.SplitterGanti,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithJarakShareLoc(int jarakShareLoc)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = this.StatusApproval,
            DirectApproval = this.DirectApproval,
            RootCause = this.RootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = jarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            SplitterGanti = this.SplitterGanti,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithJarakICrmPlus(int jarakShareLoc)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = this.StatusApproval,
            DirectApproval = this.DirectApproval,
            RootCause = this.RootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = jarakShareLoc,
            SplitterGanti = this.SplitterGanti,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithSplitterGanti(string splitterGanti)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = this.StatusApproval,
            DirectApproval = this.DirectApproval,
            RootCause = this.RootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            SplitterGanti = splitterGanti,
            VaTerbit = this.VaTerbit
        };
    }

    public ApprovalProcess WithVaTerbit(DateTime vaTerbit)
    {
        return new ApprovalProcess
        {
            SignatureApproval = this.SignatureApproval,
            StatusApproval = this.StatusApproval,
            DirectApproval = this.DirectApproval,
            RootCause = this.RootCause,
            Keterangan = this.Keterangan,
            JarakShareLoc = this.JarakShareLoc,
            JarakICrmPlus = this.JarakICrmPlus,
            SplitterGanti = this.SplitterGanti,
            VaTerbit = vaTerbit
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SignatureApproval;
        yield return StatusApproval;
        yield return DirectApproval;
        yield return RootCause;
        yield return Keterangan;
        yield return JarakShareLoc;
        yield return JarakICrmPlus;
        yield return SplitterGanti;
        yield return VaTerbit;
    }
}
