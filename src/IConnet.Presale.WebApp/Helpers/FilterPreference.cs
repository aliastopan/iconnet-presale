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
    // public bool ShowDashboardBoundary { get; set; } = false;

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

    public RootCauseExclusionModel RootCauseExclusion { get; set; } = default!;
    public ApprovalStatusExclusionModel ApprovalStatusExclusion { get; set; } = default!;

    public void ToggleFilters()
    {
        ShowFilters = !ShowFilters;
    }

    // public void ToggleDashboardBoundary()
    // {
    //     ShowDashboardBoundary = !ShowDashboardBoundary;
    // }

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

    public void SetApprovalStatusExclusion()
    {
        if (ApprovalStatusExclusion is not null)
        {
            return;
        }

        ApprovalStatusExclusion = new ApprovalStatusExclusionModel();
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
