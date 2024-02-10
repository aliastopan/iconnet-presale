namespace IConnet.Presale.WebApp.Components.Forms;

public partial class WorkloadFilterForm
{
    [Parameter] public EventCallback OnFilter { get; set; }

    public string KantorPerwakilanFilter { get; set; } = EnumerableOptions.KantorPerwakilan.First();
    public bool IsFilterSet => KantorPerwakilanFilter != EnumerableOptions.KantorPerwakilan.First();

    public async Task SetFilterAsync(string filter)
    {
        Log.Warning("Filter {0}", filter);
        KantorPerwakilanFilter = filter;

        await OnFilter.InvokeAsync();
    }
}
