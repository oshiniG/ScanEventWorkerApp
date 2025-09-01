using ScanEventWorkerApp.Application.Interfaces;
using ScanEventWorkerApp.Application.Interfaces.Repositories;
using ScanEventWorkerApp.Application.Services;
using ScanEventWorkerApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();  
builder.Services.AddHttpClient<IScanEventApiClient, ScanEventApiClient>();  
builder.Services.AddSingleton<IScanEventService, ScanEventService>();  
builder.Services.AddSingleton<IScanEventRepository, FileBasedScanEventRepository>();  

//builder.Services
//    .AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
