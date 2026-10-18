namespace TimeBinTracker;

public class CsvData
{
    public CsvDay[] Days { get; }

    public CsvData(string csv)
    {
        string[] lines = csv.Split("\n")
            .Where(x => !x.StartsWith('#'))
            .Where(x => x.Count(',') > 0)
            .ToArray();

        Days = lines.Select(x => new CsvDay(x)).ToArray();
    }

    public override string ToString()
    {
        return string.Join("\n", Days);
    }
}