using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TimeBinTracker.API.Models;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

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
            var ssmRequest = new GetParameterRequest
            {
                Name = "/time-bin-tracker/bearer-token",
                WithDecryption = true
            };

            var ssmParameter = await parameterStore.GetParameterAsync(ssmRequest);

            if (token != ssmParameter.Parameter.Value)
                return TypedResults.Unauthorized();
        }

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
