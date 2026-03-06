using System.Text;

namespace TimeBinTracker;

internal class DayActivity
{
    const int BINS_PER_HOUR = 6;
    const int HOURS_PER_DAY = 24;
    const int BINS_PER_DAY = BINS_PER_HOUR * HOURS_PER_DAY;
    readonly bool[] Activity = new bool[BINS_PER_DAY];

    public DayActivity()
    {

    }

    public DayActivity(bool[] activity)
    {
        if (activity.Length != BINS_PER_DAY)
            throw new ArgumentException("invalid length");
        Array.Copy(activity, Activity, Activity.Length);
    }

    public DayActivity(string hex)
    {
        bool[] activity = HexToBoolArray(hex);
        if (activity.Length != BINS_PER_DAY)
            throw new ArgumentException("invalid length");
        Array.Copy(activity, Activity, Activity.Length);
    }

    public static DayActivity Random()
    {
        bool[] activity = new bool[BINS_PER_DAY];
        for (int i = 0; i < BINS_PER_DAY; i++)
        {
            activity[i] = System.Random.Shared.NextDouble() > .8;
        }
        return new DayActivity(activity);
    }

    public string ToChart()
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
