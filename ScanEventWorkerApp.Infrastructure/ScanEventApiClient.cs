using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ScanEventApiClient> _logger;

        public ScanEventApiClient(HttpClient httpClient, ILogger<ScanEventApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<ScanEventModel>> FetchScanEventsAsync(int fromEventId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7253/v1/scans/scanevents?FromEventId={fromEventId}&Limit=100");
                _logger.LogInformation($"Received response: {(int)response.StatusCode} {response.ReasonPhrase}");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    options.Converters.Add(new JsonStringEnumConverter());

                    var scanEvents = JsonSerializer.Deserialize<List<ScanEventModel>>(responseContent, options);

                    return scanEvents ?? new List<ScanEventModel>();
                }

                return new List<ScanEventModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to fetch scan events from EventId {fromEventId}");
                return new List<ScanEventModel>(); 
            }
        }   
    }
}
