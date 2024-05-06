namespace IConnet.Presale.WebApp.Models.Common;

public class FilterModel
{
    public FilterModel(DateTime today, FilterPreference filterPreference,
        ICollection<string> kantorPerwakilanOptions)
    {
        FilterOfficeDefault = kantorPerwakilanOptions.First();
        FilterOffice = kantorPerwakilanOptions.First();
        FilterDaysRangeDefault = 31;

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
    public string FilterStatusApproval { get; set; } = string.Empty;

    // column filters
    public string IdPermohonan { get; set; } = string.Empty;
    public string IdPln { get; set; } = string.Empty;
    public string NamaPemohon { get; set; } = string.Empty;
    public string NomorTeleponPemohon { get; set; } = string.Empty;
    public string EmailPemohon { get; set; } = string.Empty;
    public string AlamatPemohon { get; set; } = string.Empty;

    public string Splitter { get; set; } = string.Empty;

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

    public void IdPlnFilterHandler(ChangeEventArgs args)
    {
        if (args.Value is string value)
        {
            IdPln = value;
        }
    }

    public void IdPlnFilterClear()
    {
        if (IdPln.IsNullOrWhiteSpace())
        {
            IdPln = string.Empty;
        }
    }

    public void NamaPemohonFilterHandler(ChangeEventArgs args)
    {
        if (args.Value is string value)
        {
            NamaPemohon = value;
        }
    }

    public void NamaPemohonFilterClear()
    {
        if (NamaPemohon.IsNullOrWhiteSpace())
        {
            NamaPemohon = string.Empty;
        }
    }

    public void NomorTeleponPemohonFilterHandler(ChangeEventArgs args)
    {
        if (args.Value is string value)
        {
            NomorTeleponPemohon = value;
        }
    }

    public void NomorTeleponPemohonFilterClear()
    {
        if (NomorTeleponPemohon.IsNullOrWhiteSpace())
        {
            NomorTeleponPemohon = string.Empty;
        }
    }

    public void EmailPemohonFilterHandler(ChangeEventArgs args)
    {
        if (args.Value is string value)
        {
            EmailPemohon = value;
        }
    }

    public void EmailPemohonFilterClear()
    {
        if (EmailPemohon.IsNullOrWhiteSpace())
        {
            EmailPemohon = string.Empty;
        }
    }

    public void AlamatPemohonFilterHandler(ChangeEventArgs args)
    {
        if (args.Value is string value)
        {
            AlamatPemohon = value;
        }
    }

    public void AlamatPemohonFilterClear()
    {
        if (Splitter.IsNullOrWhiteSpace())
        {
            AlamatPemohon = string.Empty;
        }
    }

    public void SplitterFilterHandler(ChangeEventArgs args)
    {
        if (args.Value is string value)
        {
            Splitter = value;
        }
    }

    public void SplitterFilterClear()
    {
        if (Splitter.IsNullOrWhiteSpace())
        {
            Splitter = string.Empty;
        }
    }

    public void ResetColumnFilters()
    {
        IdPermohonan = string.Empty;
        IdPln = string.Empty;
        NamaPemohon = string.Empty;
        NomorTeleponPemohon = string.Empty;
        EmailPemohon = string.Empty;
        AlamatPemohon = string.Empty;

        Splitter = string.Empty;
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
        IdPln = string.Empty;
        NamaPemohon = string.Empty;
        NomorTeleponPemohon = string.Empty;
        EmailPemohon = string.Empty;
        AlamatPemohon = string.Empty;
        Splitter = string.Empty;
    }
}
