namespace IConnet.Presale.WebApp.Models.Presales;

public class CustomerResponseAgingReport
{
    public CustomerResponseAgingReport(TimeSpan average, TimeSpan min, TimeSpan max)
    {
        Average = average;
        Min = min;
        Max = max;
    }

    public TimeSpan Average { get; init; }
    public TimeSpan Min { get; init; }
    public TimeSpan Max { get; init; }

    public string GetDisplayAverageAging()
    {
        return Average.ToReadableFormat();
    }

    public string GetDisplayMinAging()
    {
        return Min.ToReadableFormat();
    }

    public string GetDisplayMaxAging()
    {
        return Max.ToReadableFormat();
    }
}
