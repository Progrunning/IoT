using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AdafruitSoilMoistureReader.Core.Interfaces;
using AdafruitSoilMoistureReader.Core.Models;
using Microsoft.Azure.Devices.Client;

namespace AdafruitSoilMoistureReader.Core.Services
{
    public class IotHubService : IIotHubService
    {
        private readonly DeviceClient _deviceClient;

        public IotHubService(IAdafruitSoilMoistureReaderConfiguration adafruitSoilMoistureReaderConfiguration)
        {
            _deviceClient = DeviceClient.CreateFromConnectionString(adafruitSoilMoistureReaderConfiguration.ConnectionString);
        }

        public async Task SendAdafruitSoilMoistureReading(AdafruitSoilMoistureSensorReading reading)
        {
            await _deviceClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(reading))));
        }
    }
}