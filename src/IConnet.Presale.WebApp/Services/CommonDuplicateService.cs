namespace IConnet.Presale.WebApp.Services;

public class CommonDuplicateService
{
    private readonly List<CommonDuplicateModel> _potentialDuplicates = [];
    public List<CommonDuplicateModel> PotentialDuplicates => _potentialDuplicates;

    public void SetCommonDuplicates(IQueryable<WorkPaper>? presaleData)
    {
        if (presaleData is null)
        {
            return;
        }

        List<CommonDuplicateModel> duplicates = presaleData
            .Select(workPaper => new CommonDuplicateModel(
                workPaper.ApprovalOpportunity.IdPermohonan,
                workPaper.ApprovalOpportunity.Pemohon.IdPln,
                workPaper.ApprovalOpportunity.Pemohon.Email))
            .ToList();

        _potentialDuplicates.Clear();
        _potentialDuplicates.AddRange(duplicates);
    }
}
