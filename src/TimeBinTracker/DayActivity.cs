using System.Globalization;
using System.Text;

namespace TimeBinTracker;

public class DayActivity
{
    const int BINS_PER_HOUR = 6;
    const int HOURS_PER_DAY = 24;
    const int BINS_PER_DAY = BINS_PER_HOUR * HOURS_PER_DAY;
    readonly bool[] Activity = new bool[BINS_PER_DAY];
    public readonly DateOnly Day = DateOnly.FromDateTime(DateTime.Now);

    public DayActivity()
    {

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
    public void Set(TimeOnly time, bool active)
    {
        int hourOffset = BINS_PER_HOUR * time.Hour;
        double fractionInsideHour = time.Minute / 60.0;
        int withinHourOffset = (int)(fractionInsideHour * BINS_PER_HOUR);
        int offset = hourOffset + withinHourOffset;
        Activity[offset] = active;
    }

    public string ToChartVertical()
    {
        StringBuilder sb = new();
        for (int i = 0; i < HOURS_PER_DAY; i++)
        {
            int hourOffset = BINS_PER_HOUR * i;
            sb.Append($"{i:00} ");
            for (int j = 0; j < BINS_PER_HOUR; j++)
            {
                int offset = hourOffset + j;
                char c = Activity[offset] ? 'X' : '.';
                sb.Append(c);
            }
            sb.AppendLine();
        }
        return sb.ToString().Trim();
    }

    public string ToChartHorizontal()
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
        return sb.ToString();
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
}
