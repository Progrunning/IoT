using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using AdafruitSoilMoistureReader.Core.Interfaces;
using AdafruitSoilMoistureReader.Core.Models;

namespace AdafruitSoilMoistureReader.Core.Services
{
    public class AdafruitSoilMoistureReaderService : IAdafruitSoilMoistureReaderService
    {
        private const string AdafruitSoilMoistureReaderPythonScriptFile = "read_temp_and_soil_moisture.py";

        public async Task<AdafruitSoilMoistureSensorReading> Read()
        {
            var readTemperatureAndSoilMoistureProcessStartInfo = new ProcessStartInfo
            {
                FileName = AdafruitSoilMoistureReaderPythonScriptFile,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var readTemperatureAndSoilMoistureProcess = Process.Start(readTemperatureAndSoilMoistureProcessStartInfo);
            using var standardOutput = readTemperatureAndSoilMoistureProcess?.StandardOutput;
            using var standardError = readTemperatureAndSoilMoistureProcess?.StandardError;

            var standardErrorResult = await standardError!.ReadToEndAsync();
            var executionResult = await standardOutput!.ReadToEndAsync();

            return JsonSerializer.Deserialize<AdafruitSoilMoistureSensorReading>(executionResult);
        }
    }
}