using System.Globalization;
using System.Text;
using System.Timers;

namespace TimeBinTracker;

public class DayActivity
{
    public const int BINS_PER_HOUR = 4;
    public const int HOURS_PER_DAY = 24;
    public const int BINS_PER_DAY = BINS_PER_HOUR * HOURS_PER_DAY;
    readonly bool[] Activity = new bool[BINS_PER_DAY];
    public readonly DateOnly Day = DateOnly.FromDateTime(DateTime.Now);

    public DayActivity()
    {

    }

    public DayActivity(string dayFolder)
    {
        Day = DateOnly.Parse(Path.GetFileNameWithoutExtension(dayFolder));

        foreach (string file in Directory.GetFiles(dayFolder, "*.active"))
        {
            string filename = Path.GetFileNameWithoutExtension(file);
            string[] split = filename.Split('-');
            int hour = int.Parse(split[0]);
            int minute = int.Parse(split[1]);
            Set(hour, minute, true);
        }
    }

    public DayActivity(DateOnly day)
    {
        Day = day;
    }

    public DayActivity(DateOnly day, bool[] activity)
    {
        if (activity.Length != BINS_PER_DAY)
            throw new ArgumentException("invalid length");
        Array.Copy(activity, Activity, Activity.Length);
        Day = day;
    }

    public DayActivity(DateOnly day, string hex)
    {
        bool[] activity = HexToBoolArray(hex);
        if (activity.Length != BINS_PER_DAY)
            throw new ArgumentException("invalid length");
        Array.Copy(activity, Activity, Activity.Length);
        Day = day;
    }

    public static DayActivity Random()
    {
        bool[] activity = new bool[BINS_PER_DAY];
        for (int i = 0; i < BINS_PER_DAY; i++)
        {
            activity[i] = System.Random.Shared.NextDouble() > .8;
        }
        return new DayActivity(DateOnly.FromDateTime(DateTime.Now), activity);
    }

    public static DayActivity FromLogFolder(DateTime dt, string logFolder = "logs")
    {
        DayActivity dayActivity = new(DateOnly.FromDateTime(dt));

        string dayFolder = Path.Combine(logFolder, ActivityLogger.DateFolderName(dt));
        if (!Directory.Exists(dayFolder))
            return dayActivity; // no data for this day

        foreach (string filename in Directory.GetFiles(dayFolder, "*.active"))
        {
            DateTime dt2 = DateTime.ParseExact(
                 Path.GetFileNameWithoutExtension(filename),
                "HH-mm",
                CultureInfo.InvariantCulture
            );
            dayActivity.SetActive(TimeOnly.FromDateTime(dt2));
        }

        return dayActivity;
    }

    public void SetActive(TimeOnly time) => Set(time, true);
    public void SetInactive(TimeOnly time) => Set(time, false);
    public void Set(TimeOnly time, bool active) => Set(time.Hour, time.Minute, active);
    public void Set(int hour, int minute, bool active)
    {
        int hourOffset = BINS_PER_HOUR * hour;
        double fractionInsideHour = minute / 60.0;
        int withinHourOffset = (int)(fractionInsideHour * BINS_PER_HOUR);
        int offset = hourOffset + withinHourOffset;
        Activity[offset] = active;
    }

    public string ToChart()
    {
        static string GetHourName(int hour)
        {
            string suffix = "A";
            if (hour >= 12)
            {
                hour -= 12;
                suffix = "P";
            }

            return $"{hour:00}{suffix}";
        }

        StringBuilder sb = new();
        for (int hour = 0; hour < HOURS_PER_DAY; hour++)
        {
            string hourName = GetHourName(hour);
            sb.Append(hourName);

            sb.Append('[');
            for (int i = 0; i < BINS_PER_HOUR; i++)
            {
                sb.Append(Activity[hour * BINS_PER_HOUR + i] ? 'X' : '.');
            }
            sb.Append(']');

            sb.Append((hour + 1) % 6 == 0 ? '\n' : ' ');
        }

        return sb.ToString().Trim();
    }

    public string ToHex() => BoolArrayToHex(Activity);

    private static string BoolArrayToHex(bool[] active)
    {
        if (active.Length != 144)
            throw new ArgumentException("Array must be exactly 144 elements.");

        // 144 bits = 18 bytes
        byte[] bytes = new byte[18];

        for (int i = 0; i < active.Length; i++)
        {
            if (active[i])
                bytes[i / 8] |= (byte)(1 << (i % 8));
        }

        return Convert.ToHexString(bytes); // Returns uppercase hex, e.g. "A3F0..."
    }

    private static bool[] HexToBoolArray(string hex)
    {
        if (hex.Length != 36)
            throw new ArgumentException("Hex string must be exactly 36 characters (18 bytes).");

        byte[] bytes = Convert.FromHexString(hex);
        bool[] active = new bool[144];

        for (int i = 0; i < active.Length; i++)
        {
            active[i] = (bytes[i / 8] & (1 << (i % 8))) != 0;
        }

        return active;
    }

    public string GetDayCode()
    {
        return $"{Day.Year:0000}-{Day.Month:00}-{Day.Day:00}";
    }

    public double GetTotalActiveHours()
    {
        int activeBins = Activity.Where(x => x == true).Count();
        double hours = (double)activeBins / BINS_PER_HOUR;
        return Math.Round(hours, 1);
    }

    public string WriteAllBinsToDisk(string outputFolder)
    {
        string dayFolder = Path.Combine(outputFolder, GetDayCode());
        if (!Directory.Exists(dayFolder))
            Directory.CreateDirectory(dayFolder);

        for (int hour = 0; hour < HOURS_PER_DAY; hour++)
        {
            for (int i = 0; i < BINS_PER_HOUR; i++)
            {
                int binIndex = hour * BINS_PER_HOUR + i;
                if (Activity[binIndex])
                {
                    WriteActiveBinToDisk(dayFolder, binIndex);
                }
            }
        }

        return dayFolder;
    }

    private static void WriteActiveBinToDisk(string dayFolder, int binIndex)
    {
        int hour = binIndex / BINS_PER_HOUR;
        int binsIntoNextHour = binIndex - (hour * BINS_PER_HOUR);
        double minutes = 60.0 * binsIntoNextHour / BINS_PER_HOUR;

        string filename = $"{hour:00}-{minutes:00}.active";
        string timeFilePath = Path.Combine(dayFolder, filename);
        File.WriteAllText(timeFilePath, string.Empty);
    }

    public bool IsActive(int binIndex) => Activity[binIndex];
}
