using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdafruitSoilMoistureReader.Core.Models;
using AdafruitSoilMoistureReader.Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace AdafruitSoilMoistureReader.Functions
{
    public static class DailyAggregateFunction
    {
        [FunctionName("AggregateFunction")]
        [StorageAccount("ProgrunningIotStorageAccountConnectionString")]
        public static async void Run(
            [BlobTrigger("adafruitsoilmoisturesensor/progrunning/{partition}/{year}/{month}/{day}/{name}")] Stream capturedReading,
            [Blob("adafruitsoilmoisturesensor/progrunning/aggregations/daily/{year}/{month}/{day}", FileAccess.Read)] CloudBlobContainer dailyReadings,
            string name,
            ILogger log)
        {
            log.LogInformation($"Processing blob Name:{name} | Size: {capturedReading.Length} Bytes");

            await using var capturedReadingMemoryStream = new MemoryStream();
            await capturedReading.CopyToAsync(capturedReadingMemoryStream);

            var capturedIotReadingJson = Encoding.UTF8.GetString(capturedReadingMemoryStream.ToArray());
            var capturedIotReadings = JsonConvert.DeserializeObject<IoTReading[]>(capturedIotReadingJson);

            var adafruitSoilMoistureAndTemperatureReadingsBytes = capturedIotReadings.Select(iotReading => Convert.FromBase64String(iotReading.Body));
            var adafruitSoilMoistureAndTemperatureReadings = new List<AdafruitSoilMoistureSensorReading>();

            foreach (var adafruitSoilMoistureAndTemperatureReadingBytes in adafruitSoilMoistureAndTemperatureReadingsBytes)
            {
                var adafruitSoilMoistureAndTemperatureReadingJson = Encoding.UTF8.GetString(adafruitSoilMoistureAndTemperatureReadingBytes);
                var adafruitSoilMoistureAndTemperatureReading = JsonConvert.DeserializeObject<AdafruitSoilMoistureSensorReading>(adafruitSoilMoistureAndTemperatureReadingJson);

                adafruitSoilMoistureAndTemperatureReadings.Add(adafruitSoilMoistureAndTemperatureReading);
            }

            var adafruitSoilMoistureAndTemperatureReadingsJson = JsonConvert.SerializeObject(adafruitSoilMoistureAndTemperatureReadings);
            var adafruitSoilMoistureAndTemperatureReadingsJsonBytes = Encoding.UTF8.GetBytes(adafruitSoilMoistureAndTemperatureReadingsJson);

            await using var dailyReadingsMemoryStream = new MemoryStream();
            await dailyReadings.CopyToAsync(capturedReadingMemoryStream);

            await dailyReadingsMemoryStream.WriteAsync(adafruitSoilMoistureAndTemperatureReadingsJsonBytes, 0, adafruitSoilMoistureAndTemperatureReadingsJsonBytes.Length);
        }
    }
}