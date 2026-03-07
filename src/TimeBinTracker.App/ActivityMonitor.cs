using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace TimeBinTracker.App;

// Use 'LibraryImportAttribute' instead of 'DllImportAttribute'
// to generate P/Invoke marshalling code at compile time
#pragma warning disable SYSLIB1054

public partial class ActivityMonitor : UserControl
{
    public EventHandler<bool>? ActivityChecked;

    private readonly System.Windows.Forms.Timer Timer;
    private readonly TimeSpan CheckInterval = Debugger.IsAttached
        ? TimeSpan.FromSeconds(10)
        : TimeSpan.FromSeconds(60);

    private Point LastPoint = Point.Empty;
    private string LastWindowTitle = string.Empty;

    public ActivityMonitor()
    {
        InitializeComponent();

        Timer = new()
        {
            Enabled = true,
            Interval = (int)CheckInterval.TotalMilliseconds,
        };

        Timer.Tick += (s, e) => CheckActivity();

        CheckActivity(); // invoke once at launch
    }

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out Point lpPoint);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    private static string GetActiveWindowTitle()
    {
        const int nChars = 256;
        StringBuilder buffer = new(nChars);
        IntPtr handle = GetForegroundWindow();

        string title = GetWindowText(handle, buffer, nChars) > 0
            ? buffer.ToString()
            : string.Empty;

        return title;
    }

    private void CheckActivity()
    {
        GetCursorPos(out Point point);
        string windowTitle = GetActiveWindowTitle();
        bool activityFound = point != LastPoint || windowTitle != LastWindowTitle;
        LastPoint = point;
        LastWindowTitle = windowTitle;
        label3.Text = $"Check interval: {CheckInterval.TotalSeconds} sec";
        label1.Text = $"Checked: {DateTime.Now}";
        label2.Text = activityFound ? "Status: Active" : "Status: Idle";
        ActivityChecked?.Invoke(this, activityFound);
    }
}
