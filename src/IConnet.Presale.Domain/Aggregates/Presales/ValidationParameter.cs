#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class ValidationParameter
{
    public ValidationParameter()
    {
        ShareLoc = new Coordinate();
    }

    public ValidationStatus PlnId { get; set; }
    public ValidationStatus NamaPelanggan { get; set; }
    public ValidationStatus NomorTelepon { get; set; }
    public ValidationStatus Email { get; set; }
    public ValidationStatus AlamatPelanggan { get; set; }
    public Coordinate ShareLoc { get; set; }
}
