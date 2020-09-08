using System;

namespace AdafruitSoilMoistureReader.Functions.Models
{
    public class SystemProperties
    {
        public string connectionAuthMethod { get; set; }

        public string connectionDeviceGenerationId { get; set; }

        public string connectionDeviceId { get; set; }

        public DateTime enqueuedTime { get; set; }
    }
}