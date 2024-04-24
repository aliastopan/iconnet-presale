namespace IConnet.Presale.WebApp.Models.Common;

public class OperatorExclusionModel
{
    public HashSet<string> Usernames { get; set; } = [];
    public HashSet<string> Inclusion { get; set; } = [];
    public HashSet<string> Exclusion => GetPacExclusions();

    public OperatorExclusionModel(List<string> usernames)
    {
        Usernames = new HashSet<string>(usernames);
        Inclusion = new HashSet<string>(usernames);
    }

    private HashSet<string> GetPacExclusions()
    {
        var exclusion = new HashSet<string>(Usernames);

        exclusion.ExceptWith(Inclusion);
        return exclusion;
    }
}
