using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GeoTracking.PositionEnricher
{
    public static class PositionEnricherFunction
    {
        private const string FunctionName = "PositionEnricher";

        [FunctionName(FunctionName)]
        public static void Run([EventHubTrigger("%eventHubName%", Connection = "eventHubConnection")]EventData myEventHubMessage, ILogger log)
        {
            log.LogInformation($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
        }
    }
}
