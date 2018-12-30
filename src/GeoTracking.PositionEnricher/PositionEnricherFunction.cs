using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GeoTracking.PositionEnricher
{
    public static class PositionEnricherFunction
    {
        private const string FunctionName = "PositionEnricher";

        private static readonly EventHubClient EhClient = EventHubClient.CreateFromConnectionString(Configuration.VesselGeoPositionConnectionString());

        [FunctionName(FunctionName)]
        public static async Task Run([EventHubTrigger("%eventHubName%", Connection = "eventHubConnection")]EventData[] messages, ILogger log)
        {
            var sendTasks = new List<Task>();

            var processor = new PositionProcessor();

            foreach (var message in messages)
            {
                var positionReport = JsonConvert.DeserializeObject<PositionReport>(Encoding.UTF8.GetString(message.Body.Array));

                var vesselGeoPosition = processor.ProcessMessage(positionReport);

                var serialized = JsonConvert.SerializeObject(
                    vesselGeoPosition,
                    Formatting.None,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                var result = new EventData(Encoding.UTF8.GetBytes(serialized));

                sendTasks.Add(EhClient.SendAsync(result));
            }

            await Task.WhenAll(sendTasks);
        }
    }

    public class PositionProcessor
    {
        public VesselGeoPosition ProcessMessage(PositionReport positionReport)
        {
            return new VesselGeoPosition(positionReport, NGeoHashAlgorithm.Default);
        }
    }
}
