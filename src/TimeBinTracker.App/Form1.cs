namespace TimeBinTracker.App;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        DayActivity da = DayActivity.Random();
        richTextBox1.Text = da.ToChart();

        Uploader up = new();
        richTextBox2.Text = up.GetPayload(da);
    }
}
