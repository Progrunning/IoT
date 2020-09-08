using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AdafruitSoilMoistureReader.Core.Interfaces;
using AdafruitSoilMoistureReader.Core.Models;
using AdafruitSoilMoistureReader.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace AdafruitSoilMoistureReader.App
{
    public class Program
    {
        private const int DefaultIntervalOfReadingDataFromSensorInMilliseconds = 60 * 1000 * 5; // 5 min

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
            var argumentsService = serviceProvider.GetService<IArgumentsService>();

            var readingIntervalInMilliseconds = DefaultIntervalOfReadingDataFromSensorInMilliseconds;
            if (argumentsService.Arguments.Length > 1 && int.TryParse(argumentsService.Arguments[1], out readingIntervalInMilliseconds))
            {
            }

            while (true)
            {
                try
                {
                    var reading = await adafruitSoilMoistureReaderService.Read();
                    await iotHubService.SendAdafruitSoilMoistureReading(reading);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Failed to read or send the data.");
                }

                await Task.Delay(readingIntervalInMilliseconds);
            }
        }
    }
}