using AdafruitSoilMoistureReader.Core.Interfaces;

namespace AdafruitSoilMoistureReader.Core.Models
{
    public class AdafruitSoilMoistureReaderConfiguration : IAdafruitSoilMoistureReaderConfiguration
    {
        public string ConnectionString { get; set; }
    }
}