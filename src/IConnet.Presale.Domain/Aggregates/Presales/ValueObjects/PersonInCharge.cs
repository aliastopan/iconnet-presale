#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class PersonInCharge : ValueObject
{
    public PersonInCharge()
    {

    }

    public PersonInCharge(string helpdesk, string planningAssetCoverage)
    {
        Helpdesk = helpdesk;
        PlanningAssetCoverage = planningAssetCoverage;
    }

    public string Helpdesk { get; init; } = string.Empty;
    public string PlanningAssetCoverage { get; init; } = string.Empty;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Helpdesk;
        yield return PlanningAssetCoverage;
    }
}
