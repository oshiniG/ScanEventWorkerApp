using ScanEventWorkerApp.Application.Interfaces;

namespace ScanEventWorkerApp.Presentation.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IScanEventService _scanEventService;
        private readonly ILogger<Worker> _logger;

        public Worker(IScanEventService scanEventService, ILogger<Worker> logger)
        {
            _scanEventService = scanEventService;
            _logger = logger;
        }

        /// <summary>
        /// This method is executed when the worker service is started.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ScanEventWorker is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _scanEventService.ProcessScanEventsAsync();
                    _logger.LogInformation("Processed scan events.");

                    // Add a delay before the next execution (every 60 seconds)
                    await Task.Delay(60000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing scan events: {ex.Message}");
                }
            }

            _logger.LogInformation("ScanEventWorker is stopping.");
        }
    }
}
