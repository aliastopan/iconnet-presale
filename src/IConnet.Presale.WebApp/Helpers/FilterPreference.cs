namespace IConnet.Presale.WebApp.Helpers;

public class FilterPreference
{
    public string KantorPerwakilan { get; set; } = EnumerableOptions.KantorPerwakilan.First();
    public bool ShowFilters { get; set; } = true;

    public void ToggleFilters()
    {
        ShowFilters = !ShowFilters;
    }
}
