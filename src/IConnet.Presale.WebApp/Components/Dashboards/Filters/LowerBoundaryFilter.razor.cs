using System.Globalization;

namespace IConnet.Presale.WebApp.Components.Dashboards.Filters;

public partial class LowerBoundaryFilter : ComponentBase
{
    [Inject] protected IToastService ToastService { get; set; } = default!;
    [Inject] protected IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] protected SessionService SessionService { get; set; } = default!;

    public DateTime CurrentLowerBoundaryDateTime { get; set; }

    public DateTime? NullableLowerBoundaryDateTime { get; set; }
    public DateTime LowerBoundaryDateTime => NullableLowerBoundaryDateTime!.Value;

    protected override void OnInitialized()
    {
        NullableLowerBoundaryDateTime = SessionService.FilterPreference.LowerBoundaryDateTime;

        CurrentLowerBoundaryDateTime = new DateTime(LowerBoundaryDateTime.Ticks);
    }

    protected string GetCurrentLowerBoundaryDate()
    {
        var cultureInfo = new CultureInfo("id-ID");
        return CurrentLowerBoundaryDateTime.ToString("dd MMM yyyy", cultureInfo);
    }

    protected void OnLowerBoundaryDateChanged(DateTime? nullableDateTime)
    {
        if (nullableDateTime is null)
        {
            return;
        }

        var upperBoundaryMin = SessionService.FilterPreference.UpperBoundaryDateTimeMin;
        var offset = DateTimeService.GetFirstDayOfWeekOffset(upperBoundaryMin);

        var upperBoundaryMinOffset = SessionService.FilterPreference.UpperBoundaryDateTimeMin.Date.AddDays(-offset);

        if (nullableDateTime.Value.Date < upperBoundaryMinOffset.Date)
        {
            OutOfRangeToast(upperBoundaryMinOffset);
            NullableLowerBoundaryDateTime = upperBoundaryMin;
        }
        else
        {
            NullableLowerBoundaryDateTime = nullableDateTime.Value;
            SessionService.FilterPreference.LowerBoundaryDateTime = LowerBoundaryDateTime;
        }
    }

    private void OutOfRangeToast(DateTime dateTime)
    {
        var intent = ToastIntent.Warning;
        var message = $"Filter tanggal di luar batas Monthly. Batas awal tersedia adalah {dateTime.ToDateOnlyFormat()}";
        var timeout = 15000;

        ToastService.ShowToast(intent, message, timeout: timeout);
    }
}
