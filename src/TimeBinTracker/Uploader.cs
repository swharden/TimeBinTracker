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
        HttpClientHandler handler = new()
        {
            AutomaticDecompression = DecompressionMethods.All
        };

        var client = new HttpClient(handler);

        client.DefaultRequestHeaders.Add("User-Agent",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
            "AppleWebKit/537.36 Chrome/120.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("Accept", "application/json, text/html, */*");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
        client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

        client.DefaultRequestHeaders.Add("Referer", "https://swharden.com/");
        client.DefaultRequestHeaders.Host = "swharden.com";

        string url = SettingURL +
            $"?ComputerId={SettingComputer}" +
            $"&Day={day.GetDayCode()}" +
            $"&Hex={day.ToHex()}" +
            $"&NoCache={Random.Shared.Next()}";

        url = $"https://swharden.com/activity/add/test.html?{DateTime.Now.Ticks}";

        var res = await client.GetAsync(url);
        string body = await res.Content.ReadAsStringAsync();

        Debug.WriteLine("Body: " + body);
        Debug.WriteLine("Status: " + res.StatusCode);
        Debug.WriteLine("Headers: " + res.Headers);
        Debug.WriteLine("Content Headers: " + res.Content.Headers);

        if (!res.IsSuccessStatusCode)
        {
            Debug.WriteLine("");
        }

        return res.StatusCode;
    }
}
