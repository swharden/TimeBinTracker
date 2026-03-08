using System.Diagnostics;
using System.Net;

namespace TimeBinTracker;

public class Uploader
{
    private readonly string SettingURL;
    private readonly string SettingComputer;
    private readonly string SettingSecret;

    public Uploader(string appFolder)
    {
        string settingsFile = Path.Combine(appFolder, "settings.txt");

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

    public async Task<HttpStatusCode> Upload(DayActivity day)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", SettingSecret);

        string url = SettingURL +
            $"?ComputerId={SettingComputer}" +
            $"&Day={day.GetDayCode()}" +
            $"&Hex={day.ToHex()}" +
            $"&Timestamp={DateTime.UtcNow:O}";

        var res = await client.GetAsync(url);

        if (!res.IsSuccessStatusCode)
        {
            Debug.WriteLine("Status: " + res.StatusCode);
            Debug.WriteLine("Body: " + await res.Content.ReadAsStringAsync());
            Debug.WriteLine("Headers: " + res.Headers);
            Debug.WriteLine("Content Headers: " + res.Content.Headers);
        }

        return res.StatusCode;
    }
}
