namespace IConnet.Presale.WebApp.Helpers;

public class WorkloadFilter
{
    private static readonly string _filterOfficeDefault = EnumerableOptions.KantorPerwakilan.First();

    // base filters
    public bool IsFilterOfficeSpecified => FilterOffice != _filterOfficeDefault;
    public string FilterOfficeDefault => _filterOfficeDefault;
    public string FilterOffice { get; set; } = _filterOfficeDefault;
    public string FilterSearch { get; set; } = string.Empty;
    public DateTime? NullableFilterDateTimeStart { get; set; } = DateTime.MinValue;
    public DateTime? NullableFilterDateTimeEnd { get; set; } = DateTime.MinValue;
    public DateTime FilterDateTimeStart => NullableFilterDateTimeStart!.Value;
    public DateTime FilterDateTimeEnd => NullableFilterDateTimeEnd!.Value;
    public TimeSpan FilterDateTimeDifference => FilterDateTimeEnd - FilterDateTimeStart;

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

    public void SetFilterDateTime(DateTime today)
    {
        NullableFilterDateTimeStart = today.AddDays(-30);
        NullableFilterDateTimeEnd = today;
    }
}
