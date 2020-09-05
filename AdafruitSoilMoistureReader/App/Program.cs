using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AdafruitSoilMoistureReader.Core.Interfaces;
using AdafruitSoilMoistureReader.Core.Models;
using AdafruitSoilMoistureReader.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AdafruitSoilMoistureReader.App
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var adafruitConfigurationJson = await File.ReadAllTextAsync("localSettings.json");
            var adafruitConfiguration = JsonSerializer.Deserialize<AdafruitSoilMoistureReaderConfiguration>(adafruitConfigurationJson);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddSerilog(Log.Logger))
                .AddSingleton<IArgumentsService>(new ArgumentService(args))
                .AddSingleton<IAdafruitSoilMoistureReaderConfiguration>(adafruitConfiguration)
                .AddSingleton<IIotHubService, IotHubService>()
                .AddSingleton<IAdafruitSoilMoistureReaderService, AdafruitSoilMoistureReaderService>()
                .BuildServiceProvider();

            var adafruitSoilMoistureReaderService = serviceProvider.GetService<IAdafruitSoilMoistureReaderService>();
            var iotHubService = serviceProvider.GetService<IIotHubService>();

            while (true)
            {
                var reading = await adafruitSoilMoistureReaderService.Read();
                await iotHubService.SendAdafruitSoilMoistureReading(reading);

                await Task.Delay(5000);
            }
        }
    }
}