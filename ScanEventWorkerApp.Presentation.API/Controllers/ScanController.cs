using Microsoft.AspNetCore.Mvc;
using ScanEventWorkerApp.Application.Interfaces;
using ScanEventWorkerApp.Application.Models;

namespace ScanEventWorkerApp.Presentation.API.Controllers
{
    [Route("v1/scans/scanevents")]
    [ApiController]
    public class ScansController : ControllerBase
    {
        private readonly IScanEventService _scanEventService;

        public ScansController(IScanEventService scanEventService)
        {
            _scanEventService = scanEventService;
        }

        // GET: v1/scans/scanevents?FromEventId=83269&Limit=100
        [HttpGet]
        public async Task<IActionResult> GetScanEvents(int? fromEventId = 1, int limit = 100)
        {
            try
            {

               var _mockScanEvents = new List<ScanEventModel>
                            {
                                new ScanEventModel
                                {
                                    EventId = 1,
                                    ParcelId = 1001,
                                    Type = Domain.Enums.EventType.PICKUP,
                                    CreatedDateTimeUtc = DateTime.UtcNow.AddMinutes(-10),
                                    StatusCode = "Success",
                                    RunId = "Run1",
                                    Device = new DeviceModel { DeviceTransactionId = 1, DeviceId = 1 },
                                    User = new UserModel { UserId = "User1", CarrierId = "NC", RunId = "100" }
                                },
                                new ScanEventModel
                                {
                                    EventId = 2,
                                    ParcelId = 1002,
                                    Type = Domain.Enums.EventType.DELIVERY,
                                    CreatedDateTimeUtc = DateTime.UtcNow,
                                    StatusCode = "Success",
                                    RunId = "Run2",
                                    Device = new DeviceModel { DeviceTransactionId = 2, DeviceId = 2 },
                                    User = new UserModel { UserId = "User2", CarrierId = "PH", RunId = "101" }
                                }
                            };
                var mockEvent = _mockScanEvents.Where(e => e.EventId == fromEventId).FirstOrDefault();

                if (mockEvent == null)
                {
                    return Ok(new List<ScanEventModel>());
                }
                var newEvents = _mockScanEvents.Where(e => e.CreatedDateTimeUtc > mockEvent.CreatedDateTimeUtc).ToList();
        
                await Task.Delay(100); 
                

                return Ok(newEvents);
            }
            catch (Exception ex)
            {
                // Handle any exceptions (like invalid data or server errors)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
