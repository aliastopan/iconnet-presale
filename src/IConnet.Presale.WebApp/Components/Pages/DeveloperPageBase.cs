namespace IConnet.Presale.WebApp.Components.Pages;

public class DeveloperPageBase : StatusTrackingPageBase
{
    [Inject] public IRedisFormatter RedisFormatter { get; set; } = default!;

    public int FormattingCount { get; set; } = 0;

    protected async Task ReformatAsync()
    {
        FormattingCount = await RedisFormatter.ReformatArchiveAsync();
    }

    public string GetFormattingResult()
    {
        if (FormattingCount == 0)
        {
            return "Not Started";
        }
            return $"Formatted {FormattingCount} entries";
    }
}
