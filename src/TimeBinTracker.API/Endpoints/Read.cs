using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TimeBinTracker.API.Models;

namespace TimeBinTracker.API.Endpoints;

public static class Read
{
    public static async Task<Ok<UpdateDays>>
    ReadDays(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        [FromServices] S3DB db)
    {
        CsvData csvData = await db.ReadData();
        UpdateDays response = UpdateDays.FromCsvData(csvData, startDate, endDate);

        return TypedResults.Ok(response);
    }
}
