using System.Text.RegularExpressions;

namespace IConnet.Presale.Domain.Common;

public class EnumProcessor
{
    public static IEnumerable<string> GetStringOptions<T>(bool skipFirst = false) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>();
        if (skipFirst)
        {
            values = values.Skip(1);
        }

        return values.Select(EnumToDisplayString);
    }

    public static string EnumToDisplayString<T>(T enumValue) where T : Enum
    {
        string enumString = enumValue.ToString();
        return Regex.Replace(enumString, "([a-z])([A-Z])", "$1 $2");
    }

    public static string DisplayStringToEnumString(string displayString)
    {
        return Regex.Replace(displayString, " ", "");
    }

    public static T StringToEnum<T>(string displayString) where T : Enum
    {
        string enumString = Regex.Replace(displayString, " ", "");

        try
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException($"The string '{displayString}' could not be converted to enum type '{typeof(T).Name}'.", nameof(displayString));
        }
    }

    public static List<T> GetAllEnumValues<T>() where T : Enum
    {
        return new List<T>((T[])Enum.GetValues(typeof(T)));
    }
}
