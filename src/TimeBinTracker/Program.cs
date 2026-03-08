using TimeBinTracker;

string logFolderIn = @"C:\Users\sharden\source\repos\ActiveWindowLogger\BUILD2\logs";
string logFolderOut = Path.GetFullPath("converted");
Import.ActiveWindowLogger(logFolderIn, logFolderOut);