namespace IConnet.Presale.Domain.Common;

public class EnumHelper
{
    public static IEnumerable<string> GetIEnumerable<T>(bool skipFirst = false) where T : Enum
    {
        if (!skipFirst)
        {
            return Enum.GetNames(typeof(T));
        }

        return Enum.GetNames(typeof(T)).Skip(1);
    }
}
