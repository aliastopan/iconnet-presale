using IConnet.Presale.Shared.Interfaces.Models.Presales;

namespace IConnet.Presale.Application.Common.Interfaces.Managers;

public interface IPresaleManager
{
    Task<Result> TryInsertWorkPaper(IWorkPaperModel workPaperModel);
}
