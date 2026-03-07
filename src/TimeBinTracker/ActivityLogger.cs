namespace TimeBinTracker;

public class ActivityLogger
{
    public readonly string LogFolder;

    public ActivityLogger(string logFolder = "logs")
    {
        if (!Directory.Exists(logFolder))
            Directory.CreateDirectory(logFolder);
        LogFolder = Path.GetFullPath(logFolder);
    }

    public string GetDailyFolder(DateTime dt)
    {
        string folderName = $"";
        return Path.Join(LogFolder, folderName);
    }

    public void LogActive()
    {
        DateTime dt = DateTime.Now;
        string filename = $"{dt:yyyy}-{dt:MM}-{dt:dd}-{dt:HH}-{dt:mm}.log";
        string filePath = Path.Combine(LogFolder, filename);
        File.WriteAllText(filePath, string.Empty);
    }
}
