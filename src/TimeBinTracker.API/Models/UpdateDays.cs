namespace TimeBinTracker.API.Models;

public class UpdateDays
{
    public required List<UpdateDay> Days { get; set; }

    public static UpdateDays FromCsvData(CsvData csvData)
    {
        return FromCsvData(csvData, DateOnly.MinValue, DateOnly.MaxValue);
    }

    public static UpdateDays FromCsvData(CsvData csvData, DateOnly startDate, DateOnly endDate)
    {
        List<UpdateDay> days = [];
        foreach (CsvDay csvDay in csvData.Days)
        {
            if (csvDay.Date >= startDate && csvDay.Date <= endDate)
            {
                days.Add(new UpdateDay()
                {
                    Date = csvDay.Date,
                    Name = csvDay.Name,
                    Activity = csvDay.ActivityString,
                });
            }
        }

        return new UpdateDays() { Days = days };
    }
}
