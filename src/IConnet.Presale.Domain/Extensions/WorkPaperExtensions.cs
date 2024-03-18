using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

namespace IConnet.Presale.Domain.Extensions;

public static class WorkPaperExtensions
{
    public static bool IsDoneProcessing(this WorkPaper workPaper)
    {
        return workPaper.WorkPaperLevel == WorkPaperLevel.DoneProcessing;
    }

    public static bool IsInvalidCrmData(this WorkPaper workPaper)
    {
        return workPaper.WorkPaperLevel == WorkPaperLevel.ImportInvalid;
    }
}
