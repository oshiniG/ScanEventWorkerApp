using ScanEventWorkerApp.Application.Models;

namespace ScanEventWorkerApp.Application.Interfaces
{
    public interface IScanEventApiClient
    {
        Task<List<ScanEventModel>> FetchScanEventsAsync(int fromEventId);
    }
}
