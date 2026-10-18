using Amazon.SimpleSystemsManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TimeBinTracker.API.Models;

namespace TimeBinTracker.API.Endpoints;

public static class Delete
{
    public static async Task<Results<
        UnauthorizedHttpResult,
        Ok<UpdateDays>>>
    DeleteDay(
        [FromQuery] DateOnly day,
        [FromQuery] string name,
        HttpContext context,
        [FromServices] S3DB db,
        [FromServices] IAmazonSimpleSystemsManagement parameterStore)
    {
        if (!await RequestAuthorization.IsAuthorized(context, parameterStore))
            return TypedResults.Unauthorized();

        CsvData csvData = await db.ReadData();
        csvData.Days.RemoveAll(csvDay => csvDay.Date == day && csvDay.Name == name);
        await db.WriteData(csvData);

        UpdateDays days = UpdateDays.FromCsvData(csvData);
        return TypedResults.Ok(days);
    }
}
