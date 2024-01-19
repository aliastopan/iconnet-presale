#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales;

public class ValidationParameter
{
    public string PhoneNumber { get; set; }
    public string CustomerName { get; set; }
    public string EmailAddress { get; set; }
    public string CustomerAddress { get; set; }
    public string PlnId { get; set; }
    public string ShareLoc { get; set; }
}
