using System.Text;

namespace TimeBinTracker;

internal class Report()
{
    public static void Generate(string logFolder, string outputFile = "report.html")
    {
        // Populate all days in memory
        List<DayActivity> days = [];
        foreach (string dayFolder in Directory.GetDirectories(logFolder))
        {
            days.Add(new DayActivity(dayFolder));
        }

        // Add blanks for missing days
        DateOnly[] daysSeen = days.Select(x => x.Day).ToArray();
        DateOnly firstDay = days.Min(x => x.Day);
        DateOnly lastDay = days.Max(x => x.Day);
        int daysCovered = lastDay.DayNumber - firstDay.DayNumber;
        for (int i = 0; i < daysCovered; i++)
        {
            DateOnly date = firstDay.AddDays(i);
            if (!daysSeen.Contains(date))
            {
                days.Add(new DayActivity(date));
            }
        }

        // Generate the report on sorted data with empty days
        StringBuilder sb = new();
        foreach (DayActivity day in days.OrderBy(x => x.Day))
        {
            Console.WriteLine();
            Console.WriteLine(day.GetDayCode());
            Console.WriteLine(day.ToChart());
            sb.AppendLine(GetDayHtml(day));
        }

        string html = WrapInPage(sb.ToString());
        File.WriteAllText(outputFile, html);
        Console.WriteLine(Path.GetFullPath(outputFile));
    }

    private static string GetDayHtml(DayActivity day)
    {
        DateOnly date = DateOnly.Parse(day.GetDayCode());

        StringBuilder sb = new();

        for (int hour = 0; hour < DayActivity.HOURS_PER_DAY; hour++)
        {
            sb.AppendLine("<div class=\"sq-hour\">");
            for (int hourBin = 0; hourBin < DayActivity.BINS_PER_HOUR; hourBin++)
            {
                int binIndex = hour * DayActivity.BINS_PER_HOUR + hourBin;
                if (day.IsActive(binIndex))
                {
                    sb.AppendLine("<span class=\"sq sq-1\"></span>");
                }
                else
                {
                    sb.AppendLine("<span class=\"sq sq-0\"></span>");
                }
            }
            sb.AppendLine("</div>");
        }

        string weekOf = date.DayOfWeek == DayOfWeek.Monday
            ? $"<div class=\"fw-semibold mt-4\">Week of {date}</div>"
            : string.Empty;

        string sectionTemplate = $"""
            <section class="lh-lg">
                {weekOf}
                <div class="d-flex align-items-center gap-2">
                    <div class="sq-title">{date} {date.DayOfWeek} ({day.GetTotalActiveHours()} hr)</div>
                    <div class="sq-day">
                        {sb}
                    </div>
                </div>
            </section>
            """;

        return sectionTemplate;
    }

    private static string WrapInPage(string dayHtml)
    {
        return """
            <!doctype html>
            <html lang="en">

            <head>
                <meta charset="utf-8">
                <meta name="viewport" content="width=device-width, initial-scale=1">
                <title>Time Bin Tracker</title>
                <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet"
                    integrity="sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB" crossorigin="anonymous">

                <style>
                    :root {
                        --active: #2da44e;
                        --idle: #dee2e6;
                    }

                    .sq {
                        display: inline-block;
                        width: 8px;
                        height: 20px;
                        vertical-align: top;
                        border: 1px solid rgba(0, 0, 0, .05)
                    }

                    .sq-0 {
                        background-color: var(--idle);
                    }

                    .sq-1 {
                        background-color: var(--active);
                    }

                    .sq-day {
                        display: flex;
                    }

                    .sq-title {
                        width: 15rem;
                    }

                    .sq-hour {
                        display: flex;
                        outline: 1px solid rgba(0, 0, 0, .15);
                    }

                    /* hour workday starts */
                    .sq-hour:nth-of-type(9) {
                        border-right: 2px solid rgba(0, 0, 0, 1);
                    }

                    /* hour workday ends */
                    .sq-hour:nth-of-type(17) {
                        border-right: 2px solid rgba(0, 0, 0, 1);
                    }
                </style>
            </head>

            <body>
                <div class="container-fluid my-5">
                    <h1 class="text-center my-4">Time Bin Tracker</h1>

                    {{dayHtml}}

                </div>
                <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
                    integrity="sha384-FKyoEForCGlyvwx9Hj09JcYn3nv7wiPVlz7YYwJrWVcXK/BmnVDxM+D2scQbITxI"
                    crossorigin="anonymous"></script>
            </body>

            </html>
            """.Replace("{{dayHtml}}", dayHtml);
    }
}
