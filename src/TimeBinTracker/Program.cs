using TimeBinTracker;

string importFrom = @"C:\Users\sharden\source\repos\ActiveWindowLogger\BUILD2\logs";
string outputFolder = Path.GetFullPath("converted");
Directory.Delete(outputFolder, true);
Import.ActiveWindowLogger(importFrom, outputFolder);
Report.Generate(outputFolder);
