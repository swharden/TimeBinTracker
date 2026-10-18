using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TimeBinTracker.API.Models;

namespace TimeBinTracker.API.Endpoints;

public static class Update
{
    public async static Task<Results<
        UnauthorizedHttpResult,
        Ok<UpdateDays>>>
    UpdateDay(
        [FromBody] UpdateDays body,
        HttpContext context,
        [FromServices] S3DB db,
        [FromServices] ConfigService config)
    {
        string? authorization = context.Request.Headers.Authorization;

        string? token = authorization?
            .StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true
                ? authorization["Bearer ".Length..].Trim()
                : null;

        if (string.IsNullOrEmpty(token))
            return TypedResults.Unauthorized();

        if (Debugger.IsAttached)
        {
            if (token != "<local-testing>")
                return TypedResults.Unauthorized();
        }
        else
        {
            if (token != config.AuthToken)
                return TypedResults.Unauthorized();
        }

        foreach (var requestDay in body.Days)
        {
            string csvLine = requestDay.ToCsvLine();
            CsvDay day = new(csvLine);
            db.CsvData.Update(day);
        }

        UpdateDays response = UpdateDays.FromCsvData(db.CsvData);

        return TypedResults.Ok(response);
    }
}
