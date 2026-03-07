namespace TimeBinTracker.App;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        try
        {
            Application.Run(new MyApplicationContext());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "Form init error");
            throw;
        }
    }
}