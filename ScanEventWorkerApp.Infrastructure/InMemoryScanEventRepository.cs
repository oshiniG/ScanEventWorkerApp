using ScanEventWorkerApp.Application.Interfaces.Repositories;
using ScanEventWorkerApp.Application.Models;
using ScanEventWorkerApp.Domain.Entities;
using ScanEventWorkerApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanEventWorkerApp.Infrastructure
{

    public class InMemoryScanEventRepository : IScanEventRepository
    {
        private readonly Dictionary<int, ScanEvent> _scanEventStore = new();

        /// <summary>
        ///  Save a scan event to the in-memory store
        /// </summary>
        /// <param name="scanEvent"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task SaveOrUpdateEventAsync(ScanEvent scanEvent)
        {
            if (scanEvent == null)
            {
                throw new ArgumentNullException(nameof(scanEvent), "Scan event cannot be null.");
            }
            _scanEventStore[scanEvent.EventId] = scanEvent;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Get the last processed event
        /// </summary>
        /// <returns></returns>
        public Task<ScanEvent> GetLastProcessedEventAsync()
        {
            var lastEvent = _scanEventStore.Values.OrderByDescending(x => x.EventId).FirstOrDefault();
            return Task.FromResult(lastEvent);
        }

    }
}
