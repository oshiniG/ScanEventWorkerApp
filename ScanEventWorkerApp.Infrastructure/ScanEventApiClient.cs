using ScanEventWorkerApp.Application.Interfaces;
using ScanEventWorkerApp.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ScanEventWorkerApp.Infrastructure
{
    public class ScanEventApiClient : IScanEventApiClient
    {
        private readonly HttpClient _httpClient;

        public ScanEventApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ScanEventModel>> FetchScanEventsAsync(int fromEventId)
        {
            try
            {
                // Fetch events starting from the provided event ID and limit to 100
                var response = await _httpClient.GetAsync($"https://localhost:7253/v1/scans/scanevents?FromEventId={fromEventId}&Limit=100");
                var scanEvents = new List<ScanEventModel>();

                // Ensure the HTTP response is successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                var responseContent = await response.Content.ReadAsStringAsync();

                // Check if the response content is not empty
                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    options.Converters.Add(new JsonStringEnumConverter());

                    scanEvents = JsonSerializer.Deserialize<List<ScanEventModel>>(responseContent, options);
                    return scanEvents ?? new List<ScanEventModel>();
                      
                }

                return scanEvents;
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<ScanEventModel>();  // Return empty list in case of error
            }

        }
    }
}
