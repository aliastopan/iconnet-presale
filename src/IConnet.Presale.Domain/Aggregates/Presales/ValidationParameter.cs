#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class ValidationParameter
{
    public ValidationParameter()
    {
        ShareLoc = new Coordinate();
    }

    public string PlnId { get; set; }
    public string NamaPelanggan { get; set; }
    public string NomorTelepon { get; set; }
    public string Email { get; set; }
    public string AlamatPelanggan { get; set; }
    public Coordinate ShareLoc { get; set; }
}
