using ScanEventWorkerApp.Domain.Enums;
using System.Text.Json.Serialization;

namespace ScanEventWorkerApp.Application.Models
{
    public class ScanEventModel
    {
        public int EventId { get; set; }
        public int ParcelId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EventType Type { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public string StatusCode { get; set; }
        public string RunId { get; set; }
        public DeviceModel Device { get; set; }
        public UserModel User { get; set; }
    }
    public class ScanEventResponseModel
    {
        public List<ScanEventModel> ScanEvents { get; set; }
    }

    public class DeviceModel
    {
        public int DeviceTransactionId { get; set; }
        public int DeviceId { get; set; }
    }

    public class UserModel
    {
        public string UserId { get; set; }
        public string CarrierId { get; set; }
        public string RunId { get; set; }
    }
}
