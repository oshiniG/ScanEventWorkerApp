using Polly;
using Polly.Extensions.Http;
using ScanEventWorkerApp.Application.Interfaces;
using ScanEventWorkerApp.Application.Interfaces.Repositories;
using ScanEventWorkerApp.Application.Services;
using ScanEventWorkerApp.Infrastructure;
using ScanEventWorkerApp.Presentation.Worker;

var builder = Host.CreateApplicationBuilder(args);

// Register HttpClient with Polly retry Policy
builder.Services.AddHttpClient<IScanEventApiClient, ScanEventApiClient>()
    .AddTransientHttpErrorPolicy(policy => policy
        .WaitAndRetryAsync(
            3,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryAttempt, context) =>
            {
                Console.WriteLine($"Retry {retryAttempt} after {timespan.TotalSeconds}s due to {outcome.Exception?.Message}");
            }))
    .AddTransientHttpErrorPolicy(policy => policy
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30)));

// Register application services and repository
builder.Services.AddSingleton<IScanEventService, ScanEventService>();
builder.Services.AddSingleton<IScanEventRepository, FileBasedScanEventRepository>();

// Register and run the worker
builder.Services.AddSingleton<Worker>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

await host.RunAsync();