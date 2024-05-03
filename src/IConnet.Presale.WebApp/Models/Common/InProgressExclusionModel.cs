namespace IConnet.Presale.WebApp.Models.Common;

public class InProgressExclusionModel
{
    public HashSet<WorkPaperLevel> InProgressLevel { get; set; } = [];
    public HashSet<WorkPaperLevel> Inclusion { get; set; } = [];
    public HashSet<WorkPaperLevel> Exclusion => GetInProgressExclusions();

    public InProgressExclusionModel()
    {
        List<WorkPaperLevel> workPaperLevelEnums =
        [
            WorkPaperLevel.ImportUnverified,
            WorkPaperLevel.Reinstated,
            WorkPaperLevel.ImportVerified,
            WorkPaperLevel.Validating,
            WorkPaperLevel.WaitingApproval
        ];

        InProgressLevel = new HashSet<WorkPaperLevel>(workPaperLevelEnums);
        Inclusion = new HashSet<WorkPaperLevel>(workPaperLevelEnums);
    }

    private HashSet<WorkPaperLevel> GetInProgressExclusions()
    {
        var exclusion = new HashSet<WorkPaperLevel>(InProgressLevel);

        exclusion.ExceptWith(Inclusion);
        return exclusion;
    }
}
