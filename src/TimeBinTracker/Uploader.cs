namespace TimeBinTracker;

internal class Uploader
{
    private readonly string SettingURL;
    private readonly string SettingComputer;
    private readonly string SettingSecret;

    public Uploader()
    {
        string settingsFile = Path.GetFullPath("settings.txt");

        if (!File.Exists(settingsFile))
        {
            File.WriteAllText(settingsFile, """
                https://<update>
                <computer>
                <secret>
                """);
        }

        string text = File.ReadAllText(settingsFile);
        if (text.Contains(">"))
            throw new InvalidOperationException("settings file requires customization");

        string[] lines = File.ReadAllLines(settingsFile);
        if (lines.Length < 3)
            throw new InvalidOperationException("settings file expects 3 lines");

        SettingURL = lines[0];
        SettingComputer = lines[1];
        SettingSecret = lines[2];
    }

    public void Upload(DayActivity day)
    {
        var payload = new
        {
            ComputerId = SettingComputer,
            Secret = SettingSecret,
            Day = day.Day,
            Hex = day.ToHex(),
        };

        string json = System.Text.Json.JsonSerializer.Serialize(payload,
            new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });

        Console.WriteLine(json);
    }
}
