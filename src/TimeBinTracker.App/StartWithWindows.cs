using Microsoft.Win32;

namespace TimeBinTracker.App;

public static class StartWithWindows
{
    private const string AppName = "TimeBinTracker";
    private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    public static void Set(bool enable)
    {
        using RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, writable: true)
            ?? throw new NullReferenceException("unable to open registry key");

        if (enable)
        {
            // Use the full path to the executable
            string exePath = Application.ExecutablePath;
            key.SetValue(AppName, $"\"{exePath}\"");
        }
        else
        {
            // Only delete if it exists
            if (key.GetValue(AppName) != null)
                key.DeleteValue(AppName);
        }
    }

    public static bool IsEnabled()
    {
        using RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath, writable: false)
            ?? throw new NullReferenceException("unable to open registry key");

        return key?.GetValue(AppName) != null;
    }
}
