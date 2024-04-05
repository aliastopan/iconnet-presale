namespace IConnet.Presale.WebApp.Models.Common;

public class RootCauseExclusionModel
{
    public List<bool> ExclusionToggles { get; set; } = [];
    public List<string> RootCauses { get; set; } = [];
    public List<string> Exclusions { get; set; } = [];

    public RootCauseExclusionModel(ICollection<string> rootCauses)
    {
        RootCauses = new List<string>(rootCauses);

        for (int i = 0; i < RootCauses.Count; i++)
        {
            ExclusionToggles.Add(true);
        }
    }
}
