using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Infrastructure.Managers;

internal sealed class PresaleManager : IPresaleManager
{
    public Task<Result> TryInsertWorkPaper(IWorkPaperModel workPaperModel)
    {
        throw new NotImplementedException();
    }
}
