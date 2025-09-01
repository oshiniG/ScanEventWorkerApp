using Microsoft.Extensions.Logging;
using ScanEventWorkerApp.Application.Interfaces;
using ScanEventWorkerApp.Application.Interfaces.Repositories;
using ScanEventWorkerApp.Application.Models;
using ScanEventWorkerApp.Domain.Entities;
using ScanEventWorkerApp.Domain.Enums;

namespace ScanEventWorkerApp.Application.Services
{
    public class ScanEventService : IScanEventService
    {
        private readonly IScanEventRepository _scanEventRepository;
        private readonly IScanEventApiClient _scanEventApiClient;
        private readonly ILogger<ScanEventService> _logger;

        public ScanEventService(IScanEventRepository scanEventRepository, IScanEventApiClient scanEventApiClient, ILogger<ScanEventService> logger)
        {
            _scanEventRepository = scanEventRepository;
            _scanEventApiClient = scanEventApiClient;
            _logger = logger;
        }

        /// <summary>
        /// Process Scan Events
        /// </summary>
        /// <returns></returns>
        public async Task ProcessScanEventsAsync()
        {
            try
            {
                var lastEvent = await _scanEventRepository.GetLastProcessedEventAsync();
                int fromEventId = lastEvent?.EventId ?? 1;
                var scanEvents = await _scanEventApiClient.FetchScanEventsAsync(fromEventId);
                if (scanEvents == null || !scanEvents.Any())
                {
                    _logger.LogInformation("No more events found after EventId {EventId}", fromEventId);
                }
                else 
                {
                    foreach (var eventData in scanEvents)
                    {
                        var scanEvent = new ScanEvent(eventData.EventId, eventData.ParcelId, eventData.Type, eventData.CreatedDateTimeUtc, eventData.StatusCode, eventData.RunId);
                        await _scanEventRepository.SaveOrUpdateEventAsync(scanEvent);

                        _logger.LogInformation($"Processed Scan Event {eventData.EventId} for Parcel {eventData.ParcelId}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing scan events: {ex.Message}");
                throw;
            }
        }       
    }
}

