#nullable disable
using IConnet.Presale.Shared.Interfaces.Models.Common;

namespace IConnet.Presale.WebApp.Models.Common;

public class EditRootCauseModel : IEditRootCauseModel
{
    public EditRootCauseModel(Guid rootCauseId, string rootCause, string classification)
    {
        RootCauseId = rootCauseId;
        RootCause = rootCause;
        Classification = classification;
    }

    public Guid RootCauseId { get; set; }
    public string RootCause { get; set; }
    public string Classification { get; set; }
}
