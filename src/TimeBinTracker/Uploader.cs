using System.Diagnostics;
using System.Net;
using System.Text;

namespace TimeBinTracker;

public class Uploader
{
    private readonly string SettingURL;
    private readonly string SettingComputer;
    private readonly string SettingSecret;
    private readonly HttpClient HttpClient = new();

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

    public string GetPayload(DayActivity day)
    {
        var payload = new
        {
            ComputerId = SettingComputer,
            Secret = SettingSecret,
            Day = day.Day,
            Hex = day.ToHex(),
        };

        return System.Text.Json.JsonSerializer.Serialize(
            payload,
            new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
    }

    public async Task<HttpStatusCode> Upload(DayActivity day)
    {
        string json = GetPayload(day);
        StringContent content = new(json, Encoding.UTF8, "application/json");
        var res = await HttpClient.PostAsync(SettingURL, content);
        return res.StatusCode;
    }
}
