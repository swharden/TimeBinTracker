using System.Globalization;

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

    public static string DateFolderName(DateTime dt)
    {
        return $"{dt:yyyy}-{dt:MM}-{dt:dd}";
    }

    public static string TimeFileName(DateTime dt)
    {
        return $"{dt:HH}-{dt:mm}";
    }

    public void LogActive()
    {
        DateTime dt = DateTime.Now;

        string dayFolder = Path.Combine(LogFolder, DateFolderName(dt));
        if (!Directory.Exists(dayFolder))
            Directory.CreateDirectory(dayFolder);

        string timeFilePath = Path.Combine(dayFolder, $"{TimeFileName(dt)}.active");
        File.WriteAllText(timeFilePath, string.Empty);
    }
}
