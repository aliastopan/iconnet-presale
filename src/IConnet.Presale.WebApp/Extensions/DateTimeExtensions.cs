using System.Globalization;

namespace IConnet.Presale.WebApp.Extensions;

public static class DateTimeExtensions
{
    public static string ToReadableFormat(this DateTime dateTime)
    {
        var cultureInfo = new CultureInfo("id-ID");
        return dateTime.ToString("dd MMM yyyy HH:mm", cultureInfo).Replace(".", ":");;
    }
}
