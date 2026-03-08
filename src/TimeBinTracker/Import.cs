namespace TimeBinTracker;

internal class Import
{
    public static void ActiveWindowLogger(string inputFolder, string outputFolder)
    {
        string[] dailyFiles = Directory.GetFiles(inputFolder, "*.txt");
        foreach (string dailyFile in dailyFiles)
        {
            ImportDailyFile(dailyFile, outputFolder);
            break;
        }
    }

    private static void ImportDailyFile(string inputFile, string outputFolder)
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

        Console.WriteLine(day.ToChart());
    }
}
