namespace IConnet.Presale.WebApp.Models.Common;

public class RootCauseExclusionModel
{
    public HashSet<string> RootCauses { get; set; } = [];
    public HashSet<string> Inclusion { get; set; } = [];
    public HashSet<string> Exclusion => GetRootCauseExclusions();

    public RootCauseExclusionModel(ICollection<string> rootCauses)
    {
        RootCauses = new HashSet<string>(rootCauses);
        Inclusion = new HashSet<string>(rootCauses);
    }

    private HashSet<string> GetRootCauseExclusions()
    {
        var exclusion = new HashSet<string>(RootCauses);

        exclusion.ExceptWith(Inclusion);
        return exclusion;
    }
}
