using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace GeoTracking.PositionEnricher
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([EventHubTrigger("samples-workitems", Connection = "")]string myEventHubMessage, ILogger log)
        {
            log.LogInformation($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
        }
    }
}
