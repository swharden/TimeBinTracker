using System.Net;

namespace TimeBinTracker.App;

public partial class Form1 : Form
{
    readonly NotifyIcon TrayIcon;
    readonly ContextMenuStrip TrayMenu;
    readonly ActivityLogger ActivityLogger;
    readonly Uploader Uploader;

    public Form1()
    {
        InitializeComponent();

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
            System.Diagnostics.Process.Start("explorer.exe", ActivityLogger.LogFolder);
        };

        UpdateChart(); // update chart at startup
        btnUpdateChart.Click += (s, e) => UpdateChart();

        btnUpload.Click += async (s, e) => await UploadToday();

        cbStartWithWindows.Checked = StartWithWindows.IsEnabled(); // update check at startup
        cbStartWithWindows.CheckedChanged += (s, e) =>
        {
            StartWithWindows.Set(cbStartWithWindows.Checked);
        };
    }


    private void UpdateChart()
    {
        DayActivity da = DayActivity.FromLogFolder(DateTime.Today, ActivityLogger.LogFolder);
        rtbChart.Text = da.ToChartHorizontal();
        rtbPayload.Text = Uploader.GetPayload(da);
    }

    private async Task UploadToday()
    {
        DayActivity da = DayActivity.FromLogFolder(DateTime.Today, ActivityLogger.LogFolder);
        lblUploadResult.Text = "Uploading...";
        HttpStatusCode code = await Uploader.Upload(da);
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
