using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace TimeBinTracker;

public class Uploader
{
    private readonly string SettingURL;
    private readonly string SettingComputer;
    private readonly string SettingSecret;
    private readonly HttpClient HttpClient = new();
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

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
        try
        {
            string activity = string.Join(",", Enumerable.Range(0, DayActivity.HOURS_PER_DAY)
                .Select(hour => string.Concat(Enumerable.Range(0, DayActivity.BINS_PER_HOUR)
                    .Select(bin => day.IsActive(hour * DayActivity.BINS_PER_HOUR + bin) ? '1' : '0'))));

            var payload = new
            {
                days = new[]
                {
                    new { date = day.GetDayCode(), name = SettingComputer, activity }
                }
            };

            using HttpRequestMessage request = new(HttpMethod.Post, SettingURL.TrimEnd('/') + "/data")
            {
                Content = JsonContent.Create(payload, options: JsonOptions)
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", SettingSecret);

            var res = await HttpClient.SendAsync(request);

            if (!res.IsSuccessStatusCode)
            {
                Debug.WriteLine("URL: " + request.RequestUri);
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
