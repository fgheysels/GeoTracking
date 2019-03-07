using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GeoTracking.HttpFeed
{
    public static class GeoTrackingFeedFunction
    {
        [FunctionName("GeoTrackingFeed")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [EventHub("%eventHubName%", Connection = "eventHubConnection")]out EventData outputData,
            ILogger log)
        {
            outputData = null;

            if (req.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                return new OkObjectResult("HTTP feed online");
            }

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            var data = JsonConvert.DeserializeObject<PositionReport>(requestBody);

            if (data == null)
            {
                return new BadRequestObjectResult("JSON body not in expected format.");
            }

            outputData = new EventData(System.Text.Encoding.UTF8.GetBytes(requestBody));

            return new AcceptedResult();
        }
    }

    public class PositionReport
    {
        public long ObjectId { get; set; }
        public string Source { get; set; }
        public DateTime Timestamp { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
