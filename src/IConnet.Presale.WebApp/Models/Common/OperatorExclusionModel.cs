namespace IConnet.Presale.WebApp.Models.Common;

public class OperatorExclusionModel
{
    public HashSet<(Guid accountId, string username)> IdPair { get; set; } = [];
    public HashSet<string> Usernames { get; set; } = [];
    public HashSet<string> Inclusion { get; set; } = [];
    public HashSet<string> Exclusion => GetExclusions();

    public HashSet<Guid> InclusionIds => GetInclusionIds();
    public HashSet<Guid> ExclusionIds => GetExclusionIds();

    public OperatorExclusionModel(HashSet<(Guid accountId, string username)> idPair)
    {
        IdPair =  new HashSet<(Guid accountId, string username)>(idPair);
        Usernames = new HashSet<string>(idPair.Select(x => x.username));
        Inclusion = new HashSet<string>(idPair.Select(x => x.username));
    }

    public OperatorExclusionModel(OperatorExclusionModel model)
    {
        IdPair =  new HashSet<(Guid accountId, string username)>(model.IdPair);
        Usernames = new HashSet<string>(model.Usernames);
        Inclusion = new HashSet<string>(model.Inclusion);
    }

    private HashSet<string> GetExclusions()
    {
        var exclusion = new HashSet<string>(Usernames);

        exclusion.ExceptWith(Inclusion);
        return exclusion;
    }

    private HashSet<Guid> GetInclusionIds()
    {
        return IdPair
            .Where(pair => Inclusion.Contains(pair.username))
            .Select(pair => pair.accountId)
            .ToHashSet();
    }

    private HashSet<Guid> GetExclusionIds()
    {
        return IdPair
            .Where(pair => Exclusion.Contains(pair.username))
            .Select(pair => pair.accountId)
            .ToHashSet();
    }

    public void EnableAll()
    {
        Inclusion.Clear();
    }

    public void DisableAll()
    {
        Inclusion = new HashSet<string>(Usernames);
    }
}
