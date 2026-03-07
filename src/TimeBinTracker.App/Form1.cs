using System.Net;

namespace TimeBinTracker.App;

public partial class Form1 : Form
{
    private readonly NotifyIcon TrayIcon;
    private readonly ContextMenuStrip TrayMenu;
    readonly ActivityLogger ActivityLogger = new();
    readonly Uploader Uploader = new();

    public Form1()
    {
        InitializeComponent();

        TrayMenu = new ContextMenuStrip();
        TrayMenu.Items.Add("Open", null, OnOpen);
        TrayMenu.Items.Add("Exit", null, OnExit);

        TrayIcon = new NotifyIcon
        {
            Text = "My App",
            Icon = new Icon("icon.ico"),
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

        btnUpdateChart.Click += (s, e) => UpdateChart();

        // update at startup too
        UpdateChart();

        btnUpload.Click += async (s, e) => await UploadToday();
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
