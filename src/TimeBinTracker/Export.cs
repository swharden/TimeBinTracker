using System.Text;

namespace TimeBinTracker;

internal static class Export
{
    public static void JsonFile(List<DayActivity> days, string jsonFile = "activity.json")
    {
        StringBuilder sb = new();
        sb.Append('[');
        foreach (DayActivity day in days)
        {
            sb.Append('\n');
            sb.Append('{');
            sb.Append($"\"date\":\"{day.GetDayCode()}\",\"hex\":\"{day.ToHex()}\"");
            sb.Append('}');
            if (day != days.Last())
                sb.Append(',');
        }
        sb.Append('\n');
        sb.Append(']');

        File.WriteAllText(jsonFile, sb.ToString());
        Console.WriteLine(Path.GetFullPath(jsonFile));
    }
}
