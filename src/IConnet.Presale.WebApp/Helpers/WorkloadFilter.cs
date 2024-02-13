namespace IConnet.Presale.WebApp.Helpers;

public class WorkloadFilter
{
    private static readonly string _filterOfficeDefault = EnumerableOptions.KantorPerwakilan.First();
    private static readonly int _filterDaysRangeDefault = 30;

    // base filters
    public bool IsFilterOfficeSpecified => FilterOffice != _filterOfficeDefault;
    public string FilterOfficeDefault => _filterOfficeDefault;
    public string FilterOffice { get; set; } = _filterOfficeDefault;
    public string FilterSearch { get; set; } = string.Empty;
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

    public void SetFilterDateTimeDefault(DateTime today, FilterPreference filterPreference)
    {
        NullableFilterDateTimeMin = today.AddDays(-_filterDaysRangeDefault);
        NullableFilterDateTimeMax = today;

        filterPreference.SetFilterTglPermohonanDefault(FilterDateTimeMin, FilterDateTimeMax);
    }
}
