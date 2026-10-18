namespace TimeBinTracker;

public class CsvData
{
    public List<CsvDay> Days { get; }

    public CsvData(string csv)
    {
        string[] lines = csv.Split("\n")
            .Where(x => !x.StartsWith('#'))
            .Where(x => x.Count(',') > 0)
            .ToArray();

        Days = lines.Select(x => new CsvDay(x)).ToList();
    }

    public override string ToString()
    {
        return string.Join("\n", Days);
    }

    public void Update(CsvDay newDay)
    {
        // replace record if it exists already
        for (int i = 0; i < Days.Count; i++)
        {
            CsvDay existingDay = Days[i];
            if (existingDay.Date == newDay.Date && existingDay.Name == newDay.Name)
            {
                Days[i] = newDay;
                return;
            }
        }

        // add record if it does not exist
        Days.Add(newDay);
    }
}