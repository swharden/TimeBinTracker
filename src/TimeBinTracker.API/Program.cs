using Amazon.Lambda.AspNetCoreServer;
using TimeBinTracker.API.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.MapPost("/", (UpdateDays body) => Results.Ok(body));
app.MapGet("/", (DateTime? startDate, DateTime? endDate) => Results.Ok(new { startDate, endDate }));

app.Run();
