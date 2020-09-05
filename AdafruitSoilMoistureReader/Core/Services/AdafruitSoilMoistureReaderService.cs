using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AdafruitSoilMoistureReader.Core.Interfaces;
using AdafruitSoilMoistureReader.Core.Models;
using Microsoft.Extensions.Logging;

namespace AdafruitSoilMoistureReader.Core.Services
{
    public class AdafruitSoilMoistureReaderService : IAdafruitSoilMoistureReaderService
    {
#if DEBUG
        private const string AdafruitSoilMoistureReaderPythonScriptFile = "test_read_temp_and_soil_moisture.py";
#else
        private const string AdafruitSoilMoistureReaderPythonScriptFile = "read_temp_and_soil_moisture.py";
#endif
        private readonly IArgumentsService _argumentsService;
        private readonly ILogger<AdafruitSoilMoistureReaderService> _logger;

        public AdafruitSoilMoistureReaderService(IArgumentsService argumentsService, ILogger<AdafruitSoilMoistureReaderService> logger)
        {
            _argumentsService = argumentsService;
            _logger = logger;
        }

        public async Task<AdafruitSoilMoistureSensorReading> Read()
        {
            var pythonExecutablePath = _argumentsService.Arguments[0];

            _logger.LogDebug($"[{nameof(Read)}] Executing script {AdafruitSoilMoistureReaderPythonScriptFile} with the python exec {pythonExecutablePath}...");

            var readTemperatureAndSoilMoistureProcessStartInfo = new ProcessStartInfo
            {
                FileName = pythonExecutablePath,
                Arguments = AdafruitSoilMoistureReaderPythonScriptFile,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var readTemperatureAndSoilMoistureProcess = Process.Start(readTemperatureAndSoilMoistureProcessStartInfo);
            using var standardOutput = readTemperatureAndSoilMoistureProcess?.StandardOutput;
            using var standardError = readTemperatureAndSoilMoistureProcess?.StandardError;

            _logger.LogDebug($"[{nameof(Read)}] Script executed successfully.");

            var standardErrorResult = await standardError!.ReadToEndAsync();
            var executionResult = await standardOutput!.ReadToEndAsync();

            _logger.LogDebug($"[{nameof(Read)}] Read the following values {executionResult}");

            return JsonSerializer.Deserialize<AdafruitSoilMoistureSensorReading>(executionResult);
        }
    }
}