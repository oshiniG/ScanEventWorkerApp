using FluentAssertions;
using ScanEventWorkerApp.Domain.Entities;
using ScanEventWorkerApp.Domain.Enums;
using ScanEventWorkerApp.Infrastructure;

namespace ScanEventWorkerApp.Test
{
    public class FileBasedScanEventRepositoryTests
    {
        private readonly FileBasedScanEventRepository _repository;

        public FileBasedScanEventRepositoryTests()
        {
            // Clean up test file before starting tests
            var testDataPath = Path.Combine(AppContext.BaseDirectory, "App_Data", "events.json");
            if (File.Exists(testDataPath))
            {
                File.Delete(testDataPath);
            }

            _repository = new FileBasedScanEventRepository();
        }

        [Fact]
        public async Task SaveOrUpdateEventAsync_ShouldSaveEventAndPersistToFile()
        {
            var scanEvent = new ScanEvent(
                            eventId: 1,
                            parcelId: 100,
                            type: EventType.PICKUP,
                            createdDateTimeUtc: DateTime.UtcNow,
                            statusCode: "Success",
                            runId: "Run1"
                        );

            await _repository.SaveOrUpdateEventAsync(scanEvent);

            var lastEvent = await _repository.GetLastProcessedEventAsync();

            lastEvent.Should().NotBeNull();
            lastEvent.EventId.Should().Be(1);

            // Check file content persisted
            var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "App_Data", "events.json"));
            json.Should().Contain("\"EventId\": 1");
        }

        [Fact]
        public async Task GetLastProcessedEventAsync_ShouldReturnNullIfNoEvents()
        {
            var lastEvent = await _repository.GetLastProcessedEventAsync();

            lastEvent.Should().BeNull();
        }
    }

}