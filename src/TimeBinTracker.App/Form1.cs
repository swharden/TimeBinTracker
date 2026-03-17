using System.Diagnostics;
using System.Net;

namespace TimeBinTracker.App;

public partial class Form1 : Form
{
    readonly NotifyIcon TrayIcon;
    readonly ContextMenuStrip TrayMenu;
    readonly ActivityLogger ActivityLogger;
    readonly Uploader Uploader;
    readonly System.Windows.Forms.Timer UploadTimer;

    public Form1()
    {
        InitializeComponent();
        Text = Version.NameAndNumber;

        string appFolder = Path.GetDirectoryName(Application.ExecutablePath)
           ?? throw new NullReferenceException("unable to determine app folder");

        ActivityLogger = new(appFolder);
        Uploader = new(appFolder);

        TrayMenu = new ContextMenuStrip();
        TrayMenu.Items.Add("Open", null, OnOpen);
        TrayMenu.Items.Add("Exit", null, OnExit);

        TrayIcon = new NotifyIcon
        {
            Text = "My App",
            Icon = new Icon(Path.Combine(appFolder, "icon.ico")),
            ContextMenuStrip = TrayMenu,
            Visible = true
        };

        TrayIcon.DoubleClick += OnOpen;
        TrayIcon.MouseClick += OnTrayIconMouseClick;

        activityMonitor1.ActivityChecked += (object? sender, bool isActive) =>
        {
            if (isActive)
            {
                ActivityLogger.LogActive();
                UpdateChart();
            }
        };

        btnOpenLogFolder.Click += (s, e) =>
        {
            Process.Start("explorer.exe", ActivityLogger.LogFolder);
        };

        UpdateChart(); // update chart at startup

        btnUpload.Click += async (s, e) => await UploadToday();

        cbStartWithWindows.Checked = StartWithWindows.IsEnabled(); // update check at startup
        cbStartWithWindows.CheckedChanged += (s, e) =>
        {
            StartWithWindows.Set(cbStartWithWindows.Checked);
        };

        double uploadIntervalMinutes = Debugger.IsAttached ? 0.5 : 5.0;
        const int MILLISCONDS_PER_MINUTE = 1000 * 60;
        UploadTimer = new()
        {
            Interval = (int)(uploadIntervalMinutes * MILLISCONDS_PER_MINUTE),
            Enabled = true,
        };

        UploadTimer.Tick += async (s, e) => await UploadToday();
    }


    private void UpdateChart()
    {
        DayActivity da = DayActivity.FromLogFolder(DateTime.Today, ActivityLogger.LogFolder);
        rtbChart.Text = da.ToChart();
    }

    private async Task UploadToday()
    {
        DayActivity da = DayActivity.FromLogFolder(DateTime.Today, ActivityLogger.LogFolder);
        lblUploadResult.Text = "Uploading...";

        HttpStatusCode code = await Uploader.Upload(da);

        if (code == HttpStatusCode.Forbidden || code == HttpStatusCode.FailedDependency)
        {
            // Retry once if unauthorized.
            // CloudFlare has issues with the first attempt sometimes.
            lblUploadResult.Text = "Retrying...";
            Application.DoEvents();
            Thread.Sleep(1000);
            code = await Uploader.Upload(da);
        }

        lblUploadResult.Text = code == HttpStatusCode.OK
            ? $"Success {TimeOnly.FromDateTime(DateTime.Now)}"
            : $"Error {TimeOnly.FromDateTime(DateTime.Now)} {code}";
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            // just hide the window but keep it running in the system tray
            e.Cancel = true;
            Hide();
        }
        else
        {
            base.OnFormClosing(e);
        }
    }

    private void OnTrayIconMouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            var method = typeof(NotifyIcon)
                .GetMethod("ShowContextMenu",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic);
            method?.Invoke(TrayIcon, null);
        }
    }

    private void OnOpen(object? sender, EventArgs e)
    {
        Show();
        WindowState = FormWindowState.Normal;
        BringToFront();
    }

    private void OnExit(object? sender, EventArgs e)
    {
        TrayIcon.Visible = false;
        Application.Exit();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        TrayIcon.Dispose();
        base.OnFormClosed(e);
    }
}
