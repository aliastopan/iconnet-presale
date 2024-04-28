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

    public RootCauseExclusionModel(RootCauseExclusionModel model)
    {
        RootCauses = new HashSet<string>(model.RootCauses);
        Inclusion = new HashSet<string>(model.Inclusion);
    }

    private HashSet<string> GetRootCauseExclusions()
    {
        var exclusion = new HashSet<string>(RootCauses);

        exclusion.ExceptWith(Inclusion);
        return exclusion;
    }

    public void EnableAll()
    {
        Inclusion.Clear();
    }

    public void DisableAll()
    {
        Inclusion = new HashSet<string>(RootCauses);
    }
}
