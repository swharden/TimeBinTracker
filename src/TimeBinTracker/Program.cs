using TimeBinTracker;

// Create modern timestamp files from ActiveWindowLogger logs
string importFrom = @"C:\Users\sharden\source\repos\ActiveWindowLogger\BUILD2\logs";
string outputFolder = Path.GetFullPath("converted");
Directory.Delete(outputFolder, true);
Import.ActiveWindowLogger(importFrom, outputFolder);

// Load activity from filesystem files
List<DayActivity> days = Import.LoadDays(outputFolder);
foreach (var day in days)
{
    Console.WriteLine();
    Console.WriteLine(day.GetDayCode());
    Console.WriteLine(day.ToChart());
}

// Create JSON file JS could import
Export.JsonFile(days);

// Generate a flat-file HTML report
Report.Generate(days);
