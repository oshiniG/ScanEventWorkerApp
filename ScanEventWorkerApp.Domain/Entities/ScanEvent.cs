using ScanEventWorkerApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanEventWorkerApp.Domain.Entities
{
    public class ScanEvent
    {
        public ScanEvent(int eventId, int parcelId, EventType type, DateTime createdDateTimeUtc, string statusCode, string runId)
        {
            EventId = eventId;
            ParcelId = parcelId;
            Type = type;
            CreatedDateTimeUtc = createdDateTimeUtc;
            StatusCode = statusCode;
            RunId = runId;
        }
        public int EventId { get; set; }
        public int ParcelId { get; set; }
        public EventType Type { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public string StatusCode { get; set; }
        public string RunId { get; set; }
        public Device Device { get; set; }
        public User User { get; set; }
    }
    public class Device
    {
        public int DeviceTransactionId { get; set; }
        public int DeviceId { get; set; }
    }

    public class User
    {
        public string UserId { get; set; }
        public string CarrierId { get; set; }
        public string RunId { get; set; }
    }
}
