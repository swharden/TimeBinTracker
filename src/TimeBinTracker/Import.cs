namespace TimeBinTracker;

internal class Import
{
    public static void ActiveWindowLogger(string inputFolder, string outputFolder)
    {
        string[] dailyFiles = Directory.GetFiles(inputFolder, "*.txt");
        foreach (string dailyFile in dailyFiles)
        {
            DayActivity day = ImportDailyFile(dailyFile, outputFolder);
            string dayFolder = day.WriteAllBinsToDisk(outputFolder);
            Console.WriteLine();
            Console.WriteLine(dayFolder);
            Console.WriteLine(day.ToChart());
        }
    }

    private static DayActivity ImportDailyFile(string inputFile, string outputFolder)
    {
        string dayCode = Path.GetFileNameWithoutExtension(inputFile);
        DateOnly date = DateOnly.Parse(dayCode);
        DayActivity day = new(date);

        string[] lines = File.ReadAllLines(inputFile);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');

            if (parts.Length < 3) // invalid line
                continue;

            if (parts[1] == "ActiveWindowLogger") // internal program message
                continue;

            string start = line.Split(',')[0];
            DateTime dt = DateTime.Parse(start);
            TimeOnly time = TimeOnly.FromDateTime(dt);
            day.SetActive(time);
        }

        return day;
    }

    public static List<DayActivity> LoadDays(string logFolder)
    {
        // Populate all days in memory
        List<DayActivity> days = [];
        foreach (string dayFolder in Directory.GetDirectories(logFolder))
        {
            days.Add(new DayActivity(dayFolder));
        }

        // Add blanks for missing days
        DateOnly[] daysSeen = days.Select(x => x.Day).ToArray();
        DateOnly firstDay = days.Min(x => x.Day);
        DateOnly lastDay = days.Max(x => x.Day);
        int daysCovered = lastDay.DayNumber - firstDay.DayNumber;
        for (int i = 0; i < daysCovered; i++)
        {
            DateOnly date = firstDay.AddDays(i);
            if (!daysSeen.Contains(date))
            {
                days.Add(new DayActivity(date));
            }
        }

        return days.OrderBy(x => x.Day).ToList();
    }

}
