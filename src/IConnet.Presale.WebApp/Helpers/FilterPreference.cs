namespace IConnet.Presale.WebApp.Helpers;

public class FilterPreference
{
    public string KantorPerwakilan { get; set; } = EnumerableOptions.KantorPerwakilan.First();
    public DateTime TglPermohonanMin { get; set; } = DateTime.MinValue;
    public DateTime TglPermohonanMax { get; set; } = DateTime.MinValue;
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
