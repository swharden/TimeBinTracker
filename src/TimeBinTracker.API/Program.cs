using TimeBinTracker.API;
using TimeBinTracker.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ConfigService>();
builder.Services.AddSingleton<S3DB>();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.MapPost("/", Update.UpdateDay);
app.MapGet("/", Read.ReadDays);

app.Run();
