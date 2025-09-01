using Polly;
using ScanEventWorkerApp.Application.Interfaces;
using ScanEventWorkerApp.Application.Interfaces.Repositories;
using ScanEventWorkerApp.Application.Services;
using ScanEventWorkerApp.Infrastructure;
using ScanEventWorkerApp.Presentation.Worker;

var builder = Host.CreateApplicationBuilder(args);

// Register dependencies in DI container
builder.Services.AddHttpClient<IScanEventApiClient, ScanEventApiClient>();  
builder.Services.AddSingleton<IScanEventService, ScanEventService>();  
builder.Services.AddSingleton<IScanEventRepository, InMemoryScanEventRepository>();  
builder.Services.AddSingleton<Worker>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddHttpClient<IScanEventApiClient, ScanEventApiClient>()
    .AddTransientHttpErrorPolicy(policy => policy
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryAttempt, context) =>
            {
                Console.WriteLine($"Retry {retryAttempt} after {timespan.TotalSeconds}s due to {outcome.Exception?.Message}");
            }))
    .AddTransientHttpErrorPolicy(policy => policy
        .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));


// Build and run the host
var host = builder.Build();

await host.RunAsync();