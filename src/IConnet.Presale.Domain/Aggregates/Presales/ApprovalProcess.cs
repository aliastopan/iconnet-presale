#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class ApprovalProcess
{
    public string StatusApproval { get; set; }
    public string SebabPenolakan { get; set; }
    public string KeteranganApproval { get; set; }
    public string JarakShareLoc { get; set; }
    public string JarakICrmPlus { get; set; }
    public DateTime VaTerbit { get; set; }
}
