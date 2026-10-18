using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TimeBinTracker.API.Models;
using Amazon.SimpleSystemsManagement;

namespace TimeBinTracker.API.Endpoints;

public static class Update
{
    public static async Task<Results<
        UnauthorizedHttpResult,
        Ok<UpdateDays>>>
    UpdateDay(
        [FromBody] UpdateDays body,
        HttpContext context,
        [FromServices] S3DB db,
        [FromServices] IAmazonSimpleSystemsManagement parameterStore)
    {
        if (!await RequestAuthorization.IsAuthorized(context, parameterStore))
            return TypedResults.Unauthorized();

        CsvData csvData = await db.ReadData();

        foreach (var requestDay in body.Days)
        {
            string csvLine = requestDay.ToCsvLine();
            CsvDay day = new(csvLine);
            csvData.Update(day);
        }

        await db.WriteData(csvData);
        UpdateDays response = UpdateDays.FromCsvData(csvData);

        return TypedResults.Ok(response);
    }
}
