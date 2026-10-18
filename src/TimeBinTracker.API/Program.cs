using TimeBinTracker.API;
using TimeBinTracker.API.Endpoints;
using Amazon.SimpleSystemsManagement;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IAmazonSimpleSystemsManagement, AmazonSimpleSystemsManagementClient>();
builder.Services.AddSingleton<S3DB>();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.MapGet("/", () => $"Time Bin Tracker Lambda {TimeBinTracker.Version.Number} {Random.Shared.Next()}");
app.MapGet("/data", Read.ReadDays);
app.MapPost("/data", Update.UpdateDay);

app.Run();
