namespace TimeBinTracker;

internal class Report()
{
    public static void Generate(string logFolder, string outputFile = "report.html")
    {
        foreach (string dayFolder in Directory.GetDirectories(logFolder))
        {
            DayActivity day = new(dayFolder);
            Console.WriteLine();
            Console.WriteLine(day.GetDayCode());
            Console.WriteLine(day.ToChart());
        }
    }
}
