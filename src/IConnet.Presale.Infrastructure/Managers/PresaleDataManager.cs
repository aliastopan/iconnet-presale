using IConnet.Presale.Domain.Aggregates.Presales;
using IConnet.Presale.Domain.Enums;

namespace IConnet.Presale.Infrastructure.Managers;

internal class PresaleDataManager
{
    protected static async Task<IQueryable<WorkPaper>> FilterPartitionAsync(WorkloadFilter filter, IEnumerable<WorkPaper> partition)
    {
        return await Task.Run(() => FilterPresaleData(filter, partition.AsQueryable()));
    }

    protected static IQueryable<WorkPaper> FilterPresaleData(WorkloadFilter filter, IQueryable<WorkPaper> workPapers)
    {
        return filter switch
        {
            WorkloadFilter.OnlyImportUnverified => FilterOnly(WorkPaperLevel.ImportUnverified),
            WorkloadFilter.OnlyImportInvalid => FilterOnly(WorkPaperLevel.ImportInvalid),
            WorkloadFilter.OnlyImportArchived => FilterOnly(WorkPaperLevel.ImportArchived),
            WorkloadFilter.OnlyImportVerified => FilterOnly(WorkPaperLevel.ImportVerified),
            WorkloadFilter.OnlyValidating => FilterOnly(WorkPaperLevel.ImportVerified, WorkPaperLevel.Validating),
            WorkloadFilter.OnlyWaitingApproval => FilterOnly(WorkPaperLevel.WaitingApproval),
            WorkloadFilter.OnlyDoneProcessing => FilterOnly(WorkPaperLevel.DoneProcessing),
            _ => workPapers
        };

        IQueryable<WorkPaper> FilterOnly(params WorkPaperLevel[] levels)
        {
            if (levels.Length == 1)
            {
                return workPapers.Where(workPaper => workPaper.WorkPaperLevel == levels[0]);
            }
            else
            {
                return workPapers.Where(workPaper => levels.Any(level => workPaper.WorkPaperLevel == level));
            }
        }
    }

    protected static List<IEnumerable<WorkPaper>> SplitIntoPartitions(IEnumerable<WorkPaper> source, int partitionSize)
    {
        return source
            .Select((workPaper, index) => new
            {
                WorkPaper = workPaper,
                Index = index
            })
            .GroupBy(partition => partition.Index / partitionSize)
            .Select(group => group.Select(x => x.WorkPaper))
            .ToList();
    }
}
