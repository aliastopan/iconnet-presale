#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ApprovalProcess
{
    public string StatusApproval { get; set; }
    public string RootCause { get; set; }
    public string Keterangan { get; set; }
    public string JarakShareLoc { get; set; }
    public string JarakICrmPlus { get; set; }
    public DateTime VaTerbit { get; set; }
}
