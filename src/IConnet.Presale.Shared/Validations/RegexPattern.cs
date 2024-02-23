namespace IConnet.Presale.Shared.Validations;

public static class RegexPattern
{
    public const string Username = "^(?=.{3,16}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$";
    public const string NameFormat = "^[A-Za-z]+([-'][A-Za-z]+)*$";
    public const string StrongPassword = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$";

    public const string HomePageUrl = @"^https?:\/\/[^\/]+:[0-9]+\/$";
    public const string LatitudeLongitude = @"^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$";
}
