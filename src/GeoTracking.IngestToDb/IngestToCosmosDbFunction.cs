using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
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
            var objectIdCollectionUri = UriFactory.CreateDocumentCollectionUri("tracks", "tracks_shipid_new");
            var geohashCollectionUri = UriFactory.CreateDocumentCollectionUri("tracks", "tracks_geohash_new");

            var tasks = new List<Task<ResourceResponse<Document>>>();

            foreach (var message in messages)
            {
                var document = System.Text.Encoding.UTF8.GetString(message.Body.Array);

                object position = JsonConvert.DeserializeObject(document);

                tasks.Add(Client.CreateDocumentAsync(objectIdCollectionUri, position));
                tasks.Add(Client.CreateDocumentAsync(geohashCollectionUri, position));
            }

            await Task.WhenAll(tasks);
        }
    }
}
