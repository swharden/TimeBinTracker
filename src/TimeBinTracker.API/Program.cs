using TimeBinTracker.API;
using TimeBinTracker.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ConfigService>();
builder.Services.AddSingleton<S3DB>();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.MapGet("/", () => $"Time Bin Tracker Lambda 2.1 {Random.Shared.Next()}");
app.MapGet("/data", Read.ReadDays);
app.MapPost("/data", Update.UpdateDay);

app.Run();
