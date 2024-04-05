namespace IConnet.Presale.WebApp.Models.Common;

public class RootCauseExclusionModel
{
    public List<bool> ExclusionToggles { get; set; } = [];
    public HashSet<string> RootCauses { get; set; } = [];
    public HashSet<string> Inclusion { get; set; } = [];
    public HashSet<string> Exclusion => GetExclusionToggles();

    public RootCauseExclusionModel(ICollection<string> rootCauses)
    {
        RootCauses = new HashSet<string>(rootCauses);
        Inclusion = new HashSet<string>(rootCauses);

        for (int i = 0; i < RootCauses.Count; i++)
        {
            ExclusionToggles.Add(true);
        }
    }

    private HashSet<string> GetExclusionToggles()
    {
        var exclusion = new HashSet<string>(RootCauses);

        exclusion.ExceptWith(Inclusion);
        return exclusion;
    }
}
