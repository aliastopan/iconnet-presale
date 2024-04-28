#nullable disable

namespace IConnet.Presale.Domain.Aggregates.Presales.ValueObjects;

public class ActionSignature : ValueObject
{
    public ActionSignature()
    {
        AccountIdSignature = Guid.Empty;
        Alias = "N/A";
        TglAksi = DateTime.MinValue;
    }

    public ActionSignature(Guid accountIdSignature, string alias, DateTime tglAksi)
    {
        AccountIdSignature = accountIdSignature;
        Alias = alias;
        TglAksi = tglAksi;
    }

    public Guid AccountIdSignature { get; init; } = default;
    public string Alias { get; init; } = string.Empty;
    public DateTime TglAksi { get; init; } = default;

    public bool IsEmptySignature()
    {
        return AccountIdSignature == Guid.Empty
            || Alias == "N/A"
            || TglAksi == DateTime.MinValue;
    }

    public string ExtractUsernameFromAlias(out string role)
    {
        string[] parts = Alias.Split(' ', 2);

        if (parts.Length == 2)
        {
            role = parts[1].Trim('(', ')');
            return parts[0];
        }
        else
        {
            role = string.Empty;
            return Alias;
        }
    }

    public string GetOnlyUsernameFromAlias()
    {
        string[] parts = Alias.Split(' ', 2);

        if (parts.Length == 2)
        {
            return parts[0];
        }
        else
        {
            return Alias;
        }
    }

    public bool IsDurationExceeded(DateTime dateTimeEnd, TimeSpan duration)
    {
        var elapsedTime = dateTimeEnd - TglAksi;
        return elapsedTime > duration;
    }

    public TimeSpan GetDurationRemaining(DateTime dateTimeEnd, TimeSpan duration)
    {
        var elapsedTime = dateTimeEnd - TglAksi;
        var remainingTime = duration - elapsedTime;

        return remainingTime > TimeSpan.Zero ? remainingTime : TimeSpan.Zero;
    }

    public static ActionSignature Empty()
    {
        return new ActionSignature
        {
            AccountIdSignature = Guid.Empty,
            Alias = "N/A",
            TglAksi = DateTime.MinValue
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return AccountIdSignature;
        yield return Alias;
        yield return TglAksi;
    }
}
