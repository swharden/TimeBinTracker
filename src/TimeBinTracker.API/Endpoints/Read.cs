using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TimeBinTracker.API.Models;

namespace TimeBinTracker.API.Endpoints;

public static class Read
{
    public async static Task<Results<
        BadRequest,
        Ok<UpdateDays>>>
    ReadDays(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromServices] ConfigService config)
    {
        const string CSV = """
            #date,     00,  01,  02,  03,  04,  05,  06,  07,  08,  09,  10,  11,  12,  13,  14,  15,  16,  17,  18,  19,  20,  21,  22,  23,  name
            2026-10-12,0000,0000,0000,0000,0000,0000,0000,0000,0011,1111,1111,1111,1100,0111,1111,1111,1111,1000,0000,0000,0110,1110,0000,0000,test-name
            2026-10-13,0000,0000,0000,0000,0000,0000,0000,0001,1111,1111,1011,1111,0000,0011,1111,1101,1111,1100,0000,0000,0000,0011,1100,0000,test-name
            2026-10-14,1100,0000,0000,0000,0000,0000,0000,0000,0111,1111,1111,1110,0000,1111,1111,0011,1111,1111,1000,0000,0000,0000,0111,1111,test-name
            2026-10-15,1000,0000,0000,0000,0000,0000,0000,0000,0000,0111,1111,1111,1110,0001,1111,1111,1111,0110,0000,0000,0000,0000,0000,0000,test-name
            """;

        CsvData csvData = new(CSV);
        List<UpdateDay> days = [];
        foreach (CsvDay csvDay in csvData.Days)
        {
            days.Add(new UpdateDay()
            {
                Date = csvDay.Date,
                Name = csvDay.Name,
                Activity = csvDay.ActivityString,
            });
        }

        UpdateDays response = new() { Days = days };

        return TypedResults.Ok(response);
    }
}
