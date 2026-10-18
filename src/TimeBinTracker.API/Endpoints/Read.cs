using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TimeBinTracker.API.Models;

namespace TimeBinTracker.API.Endpoints;

public static class Read
{
    public async static Task<Ok<UpdateDays>>
    ReadDays(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate,
        [FromServices] S3DB db)
    {
        UpdateDays response = UpdateDays.FromCsvData(db.CsvData, startDate, endDate);

        return TypedResults.Ok(response);
    }
}
