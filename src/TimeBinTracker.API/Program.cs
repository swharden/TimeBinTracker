using TimeBinTracker.API;
using TimeBinTracker.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ConfigService>();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.MapPost("/", Update.UpdateDay);
app.MapGet("/", (DateTime? startDate, DateTime? endDate) => Results.Ok(new { startDate, endDate }));

app.Run();
