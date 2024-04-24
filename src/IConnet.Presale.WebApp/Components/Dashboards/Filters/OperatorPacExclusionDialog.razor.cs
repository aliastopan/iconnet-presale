namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class OperatorPacExclusionDialog : IDialogContentComponent<OperatorExclusionModel>
{
    [Inject] public SessionService SessionService { get; set; } = default!;

    [Parameter]
    public OperatorExclusionModel Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    protected bool Show { get; set; }

    protected async Task SaveAsync()
    {
        await Dialog.CloseAsync(Content);
    }

    protected async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    protected void OnExclusionChanged(string pac, bool inclusion)
    {
        if (Content.Inclusion.Count == 1)
        {
            return;
        }

        if (inclusion)
        {
            Content.Inclusion.Add(pac);
        }
        else
        {
            Content.Inclusion.Remove(pac);
        }
    }
}
