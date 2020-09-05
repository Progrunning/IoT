using System.Threading.Tasks;
using AdafruitSoilMoistureReader.Core.Models;

namespace AdafruitSoilMoistureReader.Core.Interfaces
{
    public interface IAdafruitSoilMoistureReaderService
    {
        Task<AdafruitSoilMoistureSensorReading> Read();
    }
}