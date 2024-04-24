namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class RootCauseExclusionDialog : IDialogContentComponent<RootCauseExclusionModel>
{
    [Parameter]
    public RootCauseExclusionModel Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected async Task SaveAsync()
    {
        await Dialog.CloseAsync(Content);
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    protected void OnExclusionChanged(string rootCauses, bool inclusion)
    {
        if (Content.Inclusion.Count == 1)
        {
            return;
        }

        if (inclusion)
        {
            Content.Inclusion.Add(rootCauses);
        }
        else
        {
            Content.Inclusion.Remove(rootCauses);
        }
    }
}
