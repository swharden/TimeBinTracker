using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace TimeBinTracker;

public class Uploader
{
    private readonly string SettingURL;
    private readonly string SettingComputer;
    private readonly string SettingSecret;
    private readonly HttpClient HttpClient = new();

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
        if (text.Contains('>'))
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
        string url = SettingURL +
            $"?cache={DateTime.Now.Ticks}" +
            $"&ComputerId={SettingComputer}" +
            $"&Day={day.GetDayCode()}" +
            $"&Hex={day.ToHex()}";

        try
        {
            using HttpRequestMessage request = new(HttpMethod.Get, url);
            request.Headers.Host = "swharden.com";
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SettingSecret);
            request.Headers.ConnectionClose = true;

            var res = await HttpClient.SendAsync(request);

            if (!res.IsSuccessStatusCode)
            {
                Debug.WriteLine("URL: " + url);
                Debug.WriteLine("Status: " + res.StatusCode);
                Debug.WriteLine("Body: " + await res.Content.ReadAsStringAsync());
                Debug.WriteLine("Headers: " + res.Headers);
                Debug.WriteLine("Content Headers: " + res.Content.Headers);
            }

            return res.StatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception: " + ex);
            return HttpStatusCode.FailedDependency;
        }
    }
}
