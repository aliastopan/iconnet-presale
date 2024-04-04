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

    public DateTime UpperBoundaryDateTimeMin { get; set; } = DateTime.Now.AddDays(-31);
    public DateTime UpperBoundaryDateTimeMax { get; set; } = DateTime.Now;
    public DateTime MiddleBoundaryDateTimeMin { get; set; } = DateTime.Now.AddDays(-7);
    public DateTime MiddleBoundaryDateTimeMax { get; set; } = DateTime.Now;
    public DateTime LowerBoundaryDateTime { get; set; } = DateTime.Now;

    public void ToggleFilters()
    {
        ShowFilters = !ShowFilters;
    }

    public void SetFilterTglPermohonanDefault(DateTime dateTimeMin, DateTime dateTimeMax)
    {
        TglPermohonanMin = TglPermohonanMin == DateTime.MinValue ? dateTimeMin : TglPermohonanMin;
        TglPermohonanMax = TglPermohonanMax == DateTime.MinValue ? dateTimeMax : TglPermohonanMax;
    }

    public void SetBoundaryDateTimeDefault(DateTime baselineDate)
    {
        // UpperBoundaryDateTimeMin = baselineDate.AddDays(-baselineDate.Day);
        // UpperBoundaryDateTimeMin = new DateTime(baselineDate.Year, baselineDate.Month, 1);
        UpperBoundaryDateTimeMin = baselineDate.AddDays(-31);
        UpperBoundaryDateTimeMax = baselineDate;
        MiddleBoundaryDateTimeMin = baselineDate.AddDays(-(int)baselineDate.DayOfWeek);
        MiddleBoundaryDateTimeMax = baselineDate;
        LowerBoundaryDateTime = baselineDate;
    }
}
