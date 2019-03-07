using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GeoTracking.Positions.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Spatial;
using Newtonsoft.Json;

namespace GeoTracking.Positions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private static readonly DocumentClient DbClient;

        private static readonly Uri TracksGeoCollectionUri = UriFactory.CreateDocumentCollectionUri("tracks", "tracks_geohash_new");

        static PositionsController()
        {
            DbClient = new DocumentClient(
               new Uri(CosmosDbSettings.Current.CosmosDbEndpoint), CosmosDbSettings.Current.CosmosDbKey);

            DbClient.OpenAsync().Wait();
        }

        [HttpGet]
        public  ActionResult<IEnumerable<TrackResult>> GetTracks([FromQuery]TracksByBoundingBox request)
        {
            // Due to a bug in the CosmosDb client, we need to specify the invariant-culture here.
            // The cosmos-db query will be created using the current culture settings.  
            // When querying on a double, the decimal point could otherwise be a comma instead of a point
            // which would lead to syntax-errors.
            // (see https://github.com/Azure/azure-cosmos-dotnet-v2/issues/651 )
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var searchKeys = CalculateSearchKeys(request);

            // First coordinate in the boundingbox-polygon should be the upper left corner.
            // From there, add the points counter-clockwise and finish by repeating the first location again.
            // Points must appear counter-clockwise so that st_within will know that we're interested in
            // the area inside the polygon.

            var bbox = new Polygon(
                new[]
                {
                    new Microsoft.Azure.Documents.Spatial.Position(request.MinLongitude, request.MaxLatitude),
                    new Microsoft.Azure.Documents.Spatial.Position(request.MinLongitude, request.MinLatitude),
                    new Microsoft.Azure.Documents.Spatial.Position(request.MaxLongitude, request.MinLatitude),
                    new Microsoft.Azure.Documents.Spatial.Position(request.MaxLongitude, request.MaxLatitude),
                    new Microsoft.Azure.Documents.Spatial.Position(request.MinLongitude, request.MaxLatitude),
                });

            var query = DbClient.CreateDocumentQuery<VesselGeoPosition>(TracksGeoCollectionUri, new FeedOptions()
                                {
                                    EnableCrossPartitionQuery = true
                                })
                                .Where(p => searchKeys.Contains(p.GeoHash_Date) &&
                                            p.Position.Within(bbox) &&
                                            p.Timestamp >= request.StartDate &&
                                            p.Timestamp <= request.EndDate);

            var searchResults = query.ToList();

            var groupedByVesselId = new Dictionary<long, List<VesselGeoPosition>>();

            foreach (var kvp in searchResults)
            {
                if (groupedByVesselId.ContainsKey(kvp.ShipId) == false)
                {
                    groupedByVesselId.Add(kvp.ShipId, new List<VesselGeoPosition>());
                }

                groupedByVesselId[kvp.ShipId].Add(kvp);
            }

            return Ok(groupedByVesselId.Select(kvp => new TrackResult
            {
                ShipId = kvp.Key,
                Positions = kvp.Value.Select(p => new Position() {Location = p.Position, Timestamp = p.Timestamp})
            }).ToArray());
        }

        private static IEnumerable<string> CalculateSearchKeys(TracksByBoundingBox request)
        {
            var geohashes = NGeoHash.GeoHash.Bboxes(request.MinLatitude, request.MinLongitude, request.MaxLatitude, request.MaxLongitude, 3);

            var numberOfDays = request.EndDate - request.StartDate;

            for (int i = 0; i <= numberOfDays.Days; i++)
            {
                foreach (var geohash in geohashes)
                {
                    yield return $"{geohash}_{request.StartDate.AddDays(i):yyyyMMdd}";
                }
            }
        }
    }

    public class TrackResult
    {
        public long ShipId { get; set; }
        public IEnumerable<Position> Positions
        {
            get;
            set;
        }
    }

    public class Position
    {
        public DateTime Timestamp { get; set; }
        public Point Location { get; set; }
    }

    public class VesselGeoPosition
    {
        [JsonProperty("shipId")]
        public long ShipId { get; set; }
        [JsonProperty("position")]
        public Point Position { get; set; }
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("geoHash")]
        public string GeoHash { get; set; }
        [JsonProperty("geoHash_date")]
        public string GeoHash_Date { get; set; }
    }

    public class TracksByBoundingBox
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public double MinLongitude { get; set; }
        public double MinLatitude { get; set; }
        public double MaxLongitude { get; set; }
        public double MaxLatitude { get; set; }
    }
}
