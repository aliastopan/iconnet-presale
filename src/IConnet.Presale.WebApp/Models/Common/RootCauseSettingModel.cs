#nullable disable

namespace IConnet.Presale.WebApp.Models.Common;

public record RootCauseSettingModel
{
    public Guid RootCauseId { get; init; }
    public int Order { get; init; }
    public string Cause { get; init; }
    public bool IsDeleted { get; init; }
}
