#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class ActionSignature
{
    public Guid SignatureAccountId { get; set; }
    public string SignatureAlias { get; set; }
    public DateTime ActionDateTime { get; set; }
}
