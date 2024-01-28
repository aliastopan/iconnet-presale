#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ActionSignature : ValueObject
{
    public ActionSignature()
    {

    }

    public ActionSignature(Guid accountIdSignature, string alias, DateTime tglAksi)
    {
        AccountIdSignature = accountIdSignature;
        Alias = alias;
        TglAksi = tglAksi;
    }

    public Guid AccountIdSignature { get; init; }
    public string Alias { get; init; }
    public DateTime TglAksi { get; init; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return AccountIdSignature;
        yield return Alias;
        yield return TglAksi;
    }
}
