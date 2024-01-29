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

    public string StatusApproval { get; init; } = string.Empty;
    public string RootCause { get; init; } = string.Empty;
    public string Keterangan { get; init; } = string.Empty;
    public string JarakShareLoc { get; init; } = string.Empty;
    public string JarakICrmPlus { get; init; } = string.Empty;
    public DateTime VaTerbit { get; init; } = default;

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
