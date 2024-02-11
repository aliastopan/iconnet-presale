namespace IConnet.Presale.WebApp.Helpers;

public class WorkloadFilter
{
    private static readonly string _filterOfficeDefault = EnumerableOptions.KantorPerwakilan.First();

    public bool IsFilterOfficeSpecified => FilterOffice != _filterOfficeDefault;
    public string FilterOfficeDefault => _filterOfficeDefault;
    public string FilterOffice { get; set; } = _filterOfficeDefault;
    public string FilterSearch { get; set; } = string.Empty;

    public string IdPermohonanFilter { get; set; } = string.Empty;

    public void IdPermohonanFilterHandler(ChangeEventArgs args)
    {
        Log.Warning("ChangeEventArgs");
        if (args.Value is string value)
        {
            IdPermohonanFilter = value;
        }
    }

    public void IdPermohonanFilterClear()
    {
        if (IdPermohonanFilter.IsNullOrWhiteSpace())
        {
            IdPermohonanFilter = string.Empty;
        }
    }
}
