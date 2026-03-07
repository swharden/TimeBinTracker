namespace TimeBinTracker.App;

internal class MyApplicationContext : ApplicationContext
{
    private readonly Form1 _form = new() { ShowInTaskbar = false };
    public new Form1 MainForm => _form;
}