#nullable disable

namespace IConnet.Presale.Domain.Entities;

public class RootCause
{
    public RootCause()
    {

    }

    public RootCause(int order, string cause)
    {
        Order = order;
        Cause = cause;
    }

    public RootCause(int order, string cause, string classification)
    {
        Order = order;
        Cause = cause;
        Classification = classification;
    }

    public Guid RootCauseId { get; set; }
    public int Order { get; set; }
    public string Cause { get; set; }
    public string Classification { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsOnVerification { get; set; }
}
