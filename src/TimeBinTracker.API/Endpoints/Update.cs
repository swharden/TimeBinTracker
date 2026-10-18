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
            throw new NotImplementedException(); // TODO: remove after prod token is setup
            //if (token != config.AuthToken)
                //return TypedResults.Unauthorized();
        }

        foreach (var requestDay in body.Days)
        {
            string csvLine = requestDay.ToCsvLine();
            Console.WriteLine(csvLine);
            CsvDay day = new(csvLine);
            Console.WriteLine(day);
        }

        return TypedResults.Ok();
    }
}
