using ScanEventWorkerApp.Application.Interfaces.Repositories;
using ScanEventWorkerApp.Domain.Entities;
using System.Text.Json;

namespace ScanEventWorkerApp.Infrastructure
{
    public class FileBasedScanEventRepository : IScanEventRepository
    {
        private readonly Dictionary<int, ScanEvent> _scanEventStore = new();
        private readonly string _dataFolder = Path.Combine("App_Data");
        private readonly string _dataFile = Path.Combine("App_Data", "events.json");
        private readonly object _lock = new();

        public FileBasedScanEventRepository()
        {
            if (!Directory.Exists(_dataFolder))
                Directory.CreateDirectory(_dataFolder);

            _dataFile = Path.Combine(_dataFolder, "events.json");
            LoadEventsFromFile();
        }

        /// <summary>
        ///  Save a scan event to the in-memory store and file
        /// </summary>
        /// <param name="scanEvent"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task SaveOrUpdateEventAsync(ScanEvent scanEvent)
        {
            if (scanEvent == null)
                throw new ArgumentNullException(nameof(scanEvent));

            lock (_lock)
            {
                _scanEventStore[scanEvent.EventId] = scanEvent;
                SaveEventsToFile();
            }

            return Task.CompletedTask;
        }

        public Task<ScanEvent> GetLastProcessedEventAsync()
        {
            lock (_lock)
            {
                var last = _scanEventStore.Values.OrderByDescending(e => e.EventId).FirstOrDefault();
                return Task.FromResult(last);
            }
        }

        private void SaveEventsToFile()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_scanEventStore.Values.ToList(), options);
            File.WriteAllText(_dataFile, json);
        }

        /// <summary>
        ///  Loads previously persisted scan events from a JSON file on disk into the in-memory store.
        /// </summary>
        private void LoadEventsFromFile()
        {
            if (!File.Exists(_dataFile))
                return;

            try
            {
                var json = File.ReadAllText(_dataFile);
                var events = JsonSerializer.Deserialize<List<ScanEvent>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (events != null)
                {
                    foreach (var ev in events)
                    {
                        _scanEventStore[ev.EventId] = ev;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading event data: {ex.Message}");
            }
        }

    }
}
