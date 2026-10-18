using System.Globalization;

namespace TimeBinTracker;

public class CsvDay
{
    public DateOnly Date { get; }
    public string Name { get; }
    public string ActivityString { get; }
    public bool[] ActivityByBin { get; }

    public CsvDay(string csvLine)
    {
        const int binsPerHour = 4;
        const int hoursPerDay = 24;
        const int binCount = binsPerHour * hoursPerDay;

        string[] parts = csvLine.Split(',');
        const int expectedColumnCount = hoursPerDay + 2;
        if (parts.Length != expectedColumnCount)
            throw new InvalidDataException($"CSV line contains {parts.Length} columns (expected {expectedColumnCount}):\n {csvLine}");

        Date = DateOnly.ParseExact(parts.First().Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
        Name = parts.Last().Trim();
        ActivityString = string.Join(",", parts.Skip(1).Take(hoursPerDay));

        ActivityByBin = new bool[binCount];
        for (int i = 0; i < hoursPerDay; i++)
        {
            string hourCode = parts[i + 1].Trim();
            if (hourCode.Length != binsPerHour)
                throw new InvalidDataException($"Hour code length is {hourCode.Length} (expected {binsPerHour}): {hourCode}");
            char[] codeByBin = hourCode.ToCharArray();
            int hourBinOffset = i * binsPerHour;
            for (int j = 0; j < codeByBin.Length; j++)
            {
                ActivityByBin[hourBinOffset + j] = codeByBin[j] switch
                {
                    '0' => false,
                    '1' => true,
                    _ => throw new InvalidDataException($"Invalid bin code: {codeByBin[j]}")
                };
            }
        }
    }

    public override string ToString()
    {
        return $"{Date:yyyy-MM-dd},{ActivityString},{Name}";
    }
}
