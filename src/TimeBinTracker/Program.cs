using TimeBinTracker;

//DayActivity day = DayActivity.Random();
string logFolder = @"C:\Users\swhar\Documents\personal-repos\TimeBinTracker\src\TimeBinTracker.App\bin\Debug\net10.0-windows\logs";
DayActivity day = DayActivity.FromLogFolder(DateTime.Today, logFolder);
Console.WriteLine(day.ToChartVertical());
Console.WriteLine(day.ToChartHorizontal());