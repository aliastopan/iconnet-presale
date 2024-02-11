namespace IConnet.Presale.WebApp.Helpers;

public class WorkloadFilter
{
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
