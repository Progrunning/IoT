using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AdafruitSoilMoistureReader.Core.Interfaces;
using AdafruitSoilMoistureReader.Core.Models;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;

namespace AdafruitSoilMoistureReader.Core.Services
{
    public class IotHubService : IIotHubService
    {
        private readonly DeviceClient _deviceClient;
        private readonly ILogger<IotHubService> _logger;

        public IotHubService(IAdafruitSoilMoistureReaderConfiguration adafruitSoilMoistureReaderConfiguration, ILogger<IotHubService> logger)
        {
            _logger = logger;
            _deviceClient = DeviceClient.CreateFromConnectionString(adafruitSoilMoistureReaderConfiguration.ConnectionString);
        }

        public async Task SendAdafruitSoilMoistureReading(AdafruitSoilMoistureSensorReading reading)
        {
            var readingJson = JsonSerializer.Serialize(reading);
            _logger.LogDebug($"[{nameof(SendAdafruitSoilMoistureReading)}] Sending {readingJson} message to IoT hub...");

            await _deviceClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes(readingJson)));

            _logger.LogDebug($"[{nameof(SendAdafruitSoilMoistureReading)}] Message sent.");
        }
    }
}