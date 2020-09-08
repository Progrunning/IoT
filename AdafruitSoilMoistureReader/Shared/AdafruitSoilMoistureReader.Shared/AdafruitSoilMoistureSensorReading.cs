using System;

namespace AdafruitSoilMoistureReader.Core.Models
{
    public class AdafruitSoilMoistureSensorReading
    {
        public double SoilMoisture { get; set; }

        public double Temperature { get; set; }

        public double UnixTimeStamp => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}