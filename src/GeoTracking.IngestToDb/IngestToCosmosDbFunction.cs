using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GeoTracking.IngestToDb
{
    public static class IngestToCosmosDbFunction
    {
        private static readonly DocumentClient Client = new DocumentClient(Configuration.GetCosmosDbEndpoint(), Configuration.GetCosmosDbKey());
            
        [FunctionName("IngestToCosmosDb")]
        public static async Task Run([EventHubTrigger("%eventHubName%", Connection = "eventHubConnection")]EventData[] messages, ILogger log)
        {
            var shipIdCollectionUri = UriFactory.CreateDocumentCollectionUri("tracks", "track_shipid");
            var geohashCollectionUri = UriFactory.CreateDocumentCollectionUri("tracks", "tracks_geohash");

            var tasks = new List<Task<ResourceResponse<Document>>>();

            foreach (var message in messages)
            {
                var document = System.Text.Encoding.UTF8.GetString(message.Body.Array);

                object position = JsonConvert.DeserializeObject(document);

                tasks.Add(Client.CreateDocumentAsync(shipIdCollectionUri, position));
                tasks.Add(Client.CreateDocumentAsync(geohashCollectionUri, position));
            }

            await Task.WhenAll(tasks);
        }
    }
}
