using ScanEventWorkerApp.Application.Models;

namespace ScanEventWorkerApp.Application.Interfaces
{
    public interface IScanEventService
    {
        Task ProcessScanEventsAsync();
    }
}
