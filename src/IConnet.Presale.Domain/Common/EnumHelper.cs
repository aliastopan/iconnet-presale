using System.Text.RegularExpressions;

namespace IConnet.Presale.Domain.Common;

public class EnumHelper
{
    public static IEnumerable<string> GetIEnumerable<T>(bool skipFirst = false) where T : Enum
    {
        var enumNames = Enum.GetNames(typeof(T));

        if (!skipFirst)
        {
            return enumNames.Select(EnumToDisplayString);
        }

        return enumNames.Select(EnumToDisplayString).Skip(1);
    }

    public static string EnumToDisplayString(string enumValue)
    {
        return Regex.Replace(enumValue, "([a-z])([A-Z])", "$1 $2");
    }

    public static string EnumToDisplayString<T>(T enumValue) where T : Enum
    {
        string enumString = enumValue.ToString();
        return Regex.Replace(enumString, "([a-z])([A-Z])", "$1 $2");
    }
}
