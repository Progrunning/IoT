using System;
using System.Diagnostics;

namespace AdafruitSoilMoistureReader.App
{
    internal class Program
    {
        public static string RunPythonCommand(string command, string pythonExecPath, string args)
        {
            var start = new ProcessStartInfo
            {
                FileName = pythonExecPath,
                Arguments = $"\"{command}\" \"{args}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = Process.Start(start);
            using var reader = process?.StandardOutput;

            string stderr = process?.StandardError.ReadToEnd();
            string result = reader?.ReadToEnd();
            return result;
        }

        private static void Main(string[] args)
        {
            var res = RunPythonCommand("read_temp_and_soil_moisture.py", args[0], null);
            Console.WriteLine(res);
        }
    }
}