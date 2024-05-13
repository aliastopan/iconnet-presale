namespace IConnet.Presale.WebApp.Models.Common;

public class RootCauseClassificationExclusionModel
{
    public HashSet<string> Classification { get; set; } = [];
    public HashSet<string> Inclusion { get; set; } = [];
    public HashSet<string> Exclusion => GetRootCauseExclusions();

    public RootCauseClassificationExclusionModel(ICollection<string> rootCauses)
    {
        Classification = new HashSet<string>(rootCauses);
        Inclusion = new HashSet<string>(rootCauses);
    }

    public RootCauseClassificationExclusionModel(RootCauseClassificationExclusionModel model)
    {
        Classification = new HashSet<string>(model.Classification);
        Inclusion = new HashSet<string>(model.Inclusion);
    }

    private HashSet<string> GetRootCauseExclusions()
    {
        var exclusion = new HashSet<string>(Classification);

        exclusion.ExceptWith(Inclusion);
        return exclusion;
    }

    public void EnableAll()
    {
        Inclusion.Clear();
    }

    public void DisableAll()
    {
        Inclusion = new HashSet<string>(Classification);
    }
}
