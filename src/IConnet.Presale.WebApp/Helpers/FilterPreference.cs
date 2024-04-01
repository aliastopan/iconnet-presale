namespace IConnet.Presale.WebApp.Helpers;

public class FilterPreference
{
    public FilterPreference(ICollection<string> kantorPerwakilanOptions)
    {
        KantorPerwakilan = kantorPerwakilanOptions.First();
    }

    public string KantorPerwakilan { get; set; } = string.Empty;
    public DateTime TglPermohonanMin { get; set; } = DateTime.Now.AddDays(-31);
    public DateTime TglPermohonanMax { get; set; } = DateTime.Now;
    public bool ShowFilters { get; set; } = true;

    public void ToggleFilters()
    {
        ShowFilters = !ShowFilters;
    }

    public void SetFilterTglPermohonanDefault(DateTime dateTimeMin, DateTime dateTimeMax)
    {
        TglPermohonanMin = TglPermohonanMin == DateTime.MinValue ? dateTimeMin : TglPermohonanMin;
        TglPermohonanMax = TglPermohonanMax == DateTime.MinValue ? dateTimeMax : TglPermohonanMax;
    }
}
