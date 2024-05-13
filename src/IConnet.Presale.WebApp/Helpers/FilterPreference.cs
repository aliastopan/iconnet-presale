using IConnet.Presale.WebApp.Models.Identity;

namespace IConnet.Presale.WebApp.Helpers;

public class FilterPreference
{
    public FilterPreference(ICollection<string> kantorPerwakilanOptions)
    {
        KantorPerwakilan = kantorPerwakilanOptions.First();
    }

    public string KantorPerwakilan { get; set; } = string.Empty;
    public DateTime TglPermohonanMin { get; set; } = DateTime.Now.AddDays(-31);
    public DateTime TglPermohonanMax { get; set; } = DateTime.Now;
    public bool ShowFilters { get; set; } = true;

    public DateTime UpperBoundaryDateTimeMin { get; set; } = DateTime.Now.AddDays(-31);
    public DateTime UpperBoundaryDateTimeMax { get; set; } = DateTime.Now;
    public DateTime MiddleBoundaryDateTimeMin { get; set; } = DateTime.Now.AddDays(-7);
    public DateTime MiddleBoundaryDateTimeMax { get; set; } = DateTime.Now;
    public DateTime LowerBoundaryDateTime { get; set; } = DateTime.Now;

    public Dictionary<string, BoundaryFilterMode> BoundaryFilters { get; set; } = [];
    public bool IsMonthlySelected { get; set; } = false;
    public bool IsWeeklySelected { get; set; } = false;
    public bool IsDailySelected { get; set; } = false;
    public bool IsBufferLoading { get; set; } = false;

    public void RefreshBoundaryFilters(string activeTabId)
    {
        switch (BoundaryFilters[activeTabId])
        {
            case BoundaryFilterMode.Monthly:
                ToggleToMonthlyView();
                break;
            case BoundaryFilterMode.Weekly:
                ToggleToWeeklyView();
                break;
            case BoundaryFilterMode.Daily:
                ToggleToDailyView();
                break;
            default:
                break;
        }
    }

    public InProgressExclusionModel InProgressExclusion { get; set; } = default!;
    public RootCauseExclusionModel RootCauseExclusion { get; set; } = default!;
    public ApprovalStatusExclusionModel ApprovalStatusExclusion { get; set; } = default!;
    public OperatorExclusionModel OperatorPacExclusionModel { get; set; } = default!;
    public OperatorExclusionModel OperatorHelpdeskExclusionModel { get; set; } = default!;

    public bool ShowEmptyAging { get; set; }

    public void ToggleFilters()
    {
        ShowFilters = !ShowFilters;
    }

    public bool IsRootCauseBreakdown { get; set; } = false;

    public void ToggleRootCauseBreakdown()
    {
        IsRootCauseBreakdown = !IsRootCauseBreakdown;
    }

    public void SetFilterTglPermohonanDefault(DateTime dateTimeMin, DateTime dateTimeMax)
    {
        TglPermohonanMin = TglPermohonanMin == DateTime.MinValue ? dateTimeMin : TglPermohonanMin;
        TglPermohonanMax = TglPermohonanMax == DateTime.MinValue ? dateTimeMax : TglPermohonanMax;
    }

    public void SetBoundaryDateTimeDefault(DateTime baselineDate)
    {
        // UpperBoundaryDateTimeMin = baselineDate.AddDays(-(baselineDate.Day - 1));
        // UpperBoundaryDateTimeMin = baselineDate.AddDays(-17);
        UpperBoundaryDateTimeMin = new DateTime(baselineDate.Year, baselineDate.Month, 1);
        UpperBoundaryDateTimeMax = baselineDate;
        MiddleBoundaryDateTimeMin = baselineDate.AddDays(-(int)baselineDate.DayOfWeek);
        MiddleBoundaryDateTimeMax = baselineDate;
        LowerBoundaryDateTime = baselineDate;
    }

    public void SetRootCauseExclusion(ICollection<string> rootCauses, bool allowOverwrite = false)
    {
        if (RootCauseExclusion is not null && allowOverwrite)
        {
            return;
        }

        RootCauseExclusion = new RootCauseExclusionModel(rootCauses);
    }

    public void SetInProgressExclusion()
    {
        if (InProgressExclusion is not null)
        {
            return;
        }

        InProgressExclusion = new InProgressExclusionModel();
    }

    public void SetApprovalStatusExclusion()
    {
        if (ApprovalStatusExclusion is not null)
        {
            return;
        }

        ApprovalStatusExclusion = new ApprovalStatusExclusionModel();
    }

    public void SetOperatorPacExclusionExclusion(List<PresaleOperatorModel> presaleOperators)
    {
        if (OperatorPacExclusionModel is not null)
        {
            return;
        }

        HashSet<(Guid, string)> availableOperators = presaleOperators
            .Where(p => p.UserRole == UserRole.PAC)
            .Select(p => (p.UserAccountId, p.Username))
            .ToHashSet();

        OperatorPacExclusionModel = new OperatorExclusionModel(availableOperators);
    }

    public void SetOperatorHelpdeskExclusionExclusion(List<PresaleOperatorModel> presaleOperators)
    {
        if (OperatorHelpdeskExclusionModel is not null)
        {
            return;
        }

        HashSet<(Guid, string)> availableOperators = presaleOperators
            .Where(p => p.UserRole == UserRole.Helpdesk)
            .Select(p => (p.UserAccountId, p.Username))
            .ToHashSet();

        OperatorHelpdeskExclusionModel = new OperatorExclusionModel(availableOperators);
    }

    public void ToggleToMonthlyView()
    {
        IsMonthlySelected = true;
        IsWeeklySelected = false;
        IsDailySelected = false;
    }

    public void ToggleToWeeklyView()
    {
        IsMonthlySelected = false;
        IsWeeklySelected = true;
        IsDailySelected = false;
    }

    public void ToggleToDailyView()
    {
        IsMonthlySelected = false;
        IsWeeklySelected = false;
        IsDailySelected = true;
    }
}
