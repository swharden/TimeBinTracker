using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TimeBinTracker;

const string apiBaseUrl = "https://75shuclh22byaja7qf4m2fusbm0orvqs.lambda-url.us-east-2.on.aws";
var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

Console.Write("Computer name: ");
string computerName = ReadRequiredLine();

Console.Write("Password: ");
string password = ReadPassword();

string logFolder;
while (true)
{
    Console.Write("Log data folder: ");
    logFolder = Environment.ExpandEnvironmentVariables(ReadRequiredLine());
    if (Directory.Exists(logFolder))
        break;

    Console.WriteLine("That folder does not exist. Enter a valid folder path.");
}

List<DayActivity> days;
try
{
    days = Directory.GetDirectories(logFolder)
        .Select(folder => (Folder: folder, Name: Path.GetFileName(folder)))
        .Where(item => DateOnly.TryParseExact(item.Name, "yyyy-MM-dd", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out _))
        .OrderBy(item => item.Name, StringComparer.Ordinal)
        .Select(item => new DayActivity(item.Folder))
        .ToList();
}
catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or FormatException or ArgumentException)
{
    Console.Error.WriteLine($"Could not read activity records: {ex.Message}");
    return 1;
}

if (days.Count == 0)
{
    Console.WriteLine("No dated activity folders were found; nothing to upload.");
    return 0;
}

var payload = new
{
    days = days.Select(day => new
    {
        date = day.GetDayCode(),
        name = computerName,
        activity = string.Join(",", Enumerable.Range(0, DayActivity.HOURS_PER_DAY)
            .Select(hour => string.Concat(Enumerable.Range(0, DayActivity.BINS_PER_HOUR)
                .Select(bin => day.IsActive(hour * DayActivity.BINS_PER_HOUR + bin) ? '1' : '0'))))
    }).ToArray()
};

using var httpClient = new HttpClient();
string requestBody = JsonSerializer.Serialize(payload, jsonOptions);
using var request = new HttpRequestMessage(HttpMethod.Post, $"{apiBaseUrl}/data")
{
    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
};
request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", password);

Console.WriteLine();
Console.WriteLine("API request preview:");
Console.WriteLine($"POST {request.RequestUri}");
Console.WriteLine("Authorization: Bearer [hidden]");
Console.WriteLine("Content-Type: application/json; charset=utf-8");
Console.WriteLine(requestBody);
Console.Write("Send this request? [y/N]: ");
string? confirmation = Console.ReadLine();
if (!string.Equals(confirmation?.Trim(), "y", StringComparison.OrdinalIgnoreCase)
    && !string.Equals(confirmation?.Trim(), "yes", StringComparison.OrdinalIgnoreCase))
{
    Console.WriteLine("Upload cancelled; no request was sent.");
    return 0;
}

try
{
    using HttpResponseMessage response = await httpClient.SendAsync(request);
    if (!response.IsSuccessStatusCode)
    {
        string body = await response.Content.ReadAsStringAsync();
        Console.Error.WriteLine($"Upload failed: {(int)response.StatusCode} {response.ReasonPhrase}");
        if (!string.IsNullOrWhiteSpace(body))
            Console.Error.WriteLine(body);
        return 1;
    }

    Console.WriteLine($"Uploaded {days.Count} day(s) for {computerName}.");
    return 0;
}
catch (HttpRequestException ex)
{
    Console.Error.WriteLine($"Could not reach the TimeBinTracker API: {ex.Message}");
    return 1;
}

static string ReadRequiredLine()
{
    string? value;
    while (string.IsNullOrWhiteSpace(value = Console.ReadLine()))
        Console.Write("A value is required: ");
    return value;
}

static string ReadPassword()
{
    if (Console.IsInputRedirected)
        return ReadRequiredLine();

    var password = new System.Text.StringBuilder();
    while (true)
    {
        ConsoleKeyInfo key = Console.ReadKey(intercept: true);
        if (key.Key == ConsoleKey.Enter)
        {
            Console.WriteLine();
            break;
        }
        if (key.Key == ConsoleKey.Backspace)
        {
            if (password.Length > 0)
                password.Length--;
            continue;
        }
        if (!char.IsControl(key.KeyChar))
            password.Append(key.KeyChar);
    }

    if (password.Length == 0)
    {
        Console.Write("A value is required: ");
        return ReadPassword();
    }
    return password.ToString();
}
