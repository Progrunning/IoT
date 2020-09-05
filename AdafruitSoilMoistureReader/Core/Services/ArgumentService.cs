using AdafruitSoilMoistureReader.Core.Interfaces;

namespace AdafruitSoilMoistureReader.Core.Services
{
    public class ArgumentService : IArgumentsService
    {
        public ArgumentService(string[] arguments)
        {
            Arguments = arguments;
        }

        public string[] Arguments { get; }
    }
}