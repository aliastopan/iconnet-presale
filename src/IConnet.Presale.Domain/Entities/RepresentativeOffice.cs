#nullable disable

namespace IConnet.Presale.Domain.Entities;

public class RepresentativeOffice
{
    public RepresentativeOffice()
    {
        KantorPerwakilanId = Guid.NewGuid();
    }

    public RepresentativeOffice(int order, string perwakilan)
    {
        KantorPerwakilanId = Guid.NewGuid();
        Order = order;
        Perwakilan = perwakilan;
    }

    public Guid KantorPerwakilanId { get; set; }
    public int Order { get; set; }
    public string Perwakilan { get; set; }
    public bool IsDeleted { get; set; }
}
