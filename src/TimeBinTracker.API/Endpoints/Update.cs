using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TimeBinTracker.API.Models;

namespace TimeBinTracker.API.Endpoints;

public static class Update
{
    public async static Task<Results<
        BadRequest,
        UnauthorizedHttpResult,
        Ok>>
    UpdateDay(
        [FromBody] UpdateDays body,
        HttpContext context,
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

        const string CSV = """
            #date,     00,  01,  02,  03,  04,  05,  06,  07,  08,  09,  10,  11,  12,  13,  14,  15,  16,  17,  18,  19,  20,  21,  22,  23,  name
            2026-10-12,0000,0000,0000,0000,0000,0000,0000,0000,0011,1111,1111,1111,1100,0111,1111,1111,1111,1000,0000,0000,0110,1110,0000,0000,test-name
            """;

        CsvData csvData = new(CSV);

        foreach (var requestDay in body.Days)
        {
            string csvLine = requestDay.ToCsvLine();
            CsvDay day = new(csvLine);
            csvData.Update(day);
        }

        Console.WriteLine(csvData);

        return TypedResults.Ok();
    }
}
