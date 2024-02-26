namespace IConnet.Presale.WebApp.Models.Common;

public class FilterModel
{
    public FilterModel(DateTime today, FilterPreference filterPreference,
        ICollection<string> kantorPerwakilanOptions)
    {
        FilterOfficeDefault = kantorPerwakilanOptions.First();
        FilterOffice = kantorPerwakilanOptions.First();
        FilterDaysRangeDefault = 30;

        NullableFilterDateTimeMin = today.AddDays(-FilterDaysRangeDefault);
        NullableFilterDateTimeMax = today;

        filterPreference.SetFilterTglPermohonanDefault(FilterDateTimeMin, FilterDateTimeMax);
    }

    // base filters
    public string FilterOfficeDefault = string.Empty;
    public string FilterOffice { get; set; } = string.Empty;
    public bool IsFilterOfficeSpecified => FilterOffice != FilterOfficeDefault;
    public string FilterSearch { get; set; } = string.Empty;
    public int FilterDaysRangeDefault { get; init; }
    public DateTime? NullableFilterDateTimeMin { get; set; } = DateTime.MinValue;
    public DateTime? NullableFilterDateTimeMax { get; set; } = DateTime.MinValue;
    public DateTime FilterDateTimeMin => NullableFilterDateTimeMin!.Value;
    public DateTime FilterDateTimeMax => NullableFilterDateTimeMax!.Value;
    public TimeSpan FilterDateTimeDifference => FilterDateTimeMax - FilterDateTimeMin;

    // column filters
    public string IdPermohonan { get; set; } = string.Empty;

    public void IdPermohonanFilterHandler(ChangeEventArgs args)
    {
        if (args.Value is string value)
        {
            IdPermohonan = value;
        }
    }

    public void IdPermohonanFilterClear()
    {
        if (IdPermohonan.IsNullOrWhiteSpace())
        {
            IdPermohonan = string.Empty;
        }
    }

    public void ResetColumnFilters()
    {
        IdPermohonan = string.Empty;
    }

    public void ResetFilters(DateTime today, FilterPreference filterPreference)
    {
        FilterOffice = FilterOfficeDefault;
        FilterSearch = string.Empty;
        NullableFilterDateTimeMin = today.AddDays(-FilterDaysRangeDefault);
        NullableFilterDateTimeMax = today;

        filterPreference.KantorPerwakilan = FilterOfficeDefault;
        filterPreference.TglPermohonanMin = FilterDateTimeMin;
        filterPreference.TglPermohonanMax = FilterDateTimeMax;

        IdPermohonan = string.Empty;
    }
}
