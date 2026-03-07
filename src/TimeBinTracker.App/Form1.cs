namespace TimeBinTracker.App;

public partial class Form1 : Form
{
    private readonly NotifyIcon TrayIcon;
    private readonly ContextMenuStrip TrayMenu;
    DayActivity DailyActivity = new();

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
                // TODO: account for daily rollover
                DailyActivity.SetActive(TimeOnly.FromDateTime(DateTime.Now));
                richTextBox3.Text = DailyActivity.ToChartHorizontal();
            }
        };
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        richTextBox3.Text = DailyActivity.ToChartHorizontal();

        Uploader up = new();
        richTextBox2.Text = up.GetPayload(DailyActivity);
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
