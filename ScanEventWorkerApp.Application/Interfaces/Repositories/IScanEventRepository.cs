using ScanEventWorkerApp.Application.Models;
using ScanEventWorkerApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanEventWorkerApp.Application.Interfaces.Repositories
{
    public interface IScanEventRepository
    {

        Task SaveOrUpdateEventAsync(ScanEvent scanEvent);
        Task<ScanEvent> GetLastProcessedEventAsync();
    }
}
