using System;

namespace AdafruitSoilMoistureReader.Functions.Models
{
    public class IoTReading
    {
        public string Body { get; set; }

        public DateTimeOffset EnqueuedTimeUtc { get; set; }

        public SystemProperties SystemProperties { get; set; }
    }
}