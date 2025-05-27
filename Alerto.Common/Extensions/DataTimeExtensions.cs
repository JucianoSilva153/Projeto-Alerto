namespace Alerto.Common.Extensions;

public static class StringExtensions
{
    public static DateTime ToDateTime(this string data)
    {
        var dataSplited = data.Split("/");
        return new DateTime(
            int.Parse(dataSplited[2]),
            int.Parse(dataSplited[2]),
            int.Parse(dataSplited[2]));
    }
}