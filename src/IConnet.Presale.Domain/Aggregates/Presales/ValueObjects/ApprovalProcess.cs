#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ApprovalProcess : ValueObject
{
    public ApprovalProcess()
    {

    }

    public ApprovalProcess(string statusApproval, string rootCause, string keterangan,
        string jarakShareLoc, string jarakICrmPlus, DateTime vaTerbit)
    {
        StatusApproval = statusApproval;
        RootCause = rootCause;
        Keterangan = keterangan;
        JarakShareLoc = jarakShareLoc;
        JarakICrmPlus = jarakICrmPlus;
        VaTerbit = vaTerbit;
    }

    public string StatusApproval { get; init; }
    public string RootCause { get; init; }
    public string Keterangan { get; init; }
    public string JarakShareLoc { get; init; }
    public string JarakICrmPlus { get; init; }
    public DateTime VaTerbit { get; init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StatusApproval;
        yield return RootCause;
        yield return Keterangan;
        yield return JarakShareLoc;
        yield return JarakICrmPlus;
        yield return VaTerbit;
    }
}
