#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class ActionSignature
{
    public Guid AccountIdSignature { get; set; }
    public string Alias { get; set; }
    public DateTime TglAksi { get; set; }
}
