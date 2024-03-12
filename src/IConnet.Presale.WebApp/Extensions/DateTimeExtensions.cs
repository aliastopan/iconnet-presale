using System;
using System.Globalization;

namespace IConnet.Presale.WebApp.Extensions;

public static class DateTimeExtensions
{
    public static string ToReadableFormat(this DateTime dateTime)
    {
        return dateTime.ToString("dd MMM yyyy HH:mm", CultureInfo.InvariantCulture);
    }
}
