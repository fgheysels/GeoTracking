using System;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;

namespace GeoTracking.PositionEnricher
{
    public class VesselGeoPosition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselGeoPosition"/> class.
        /// </summary>
        public VesselGeoPosition(PositionReport position, IGeoHashAlgorithm geoHasher) 
        : this(position.ShipId, position.Source, position.Longitude, position.Latitude, position.Timestamp, geoHasher)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselGeoPosition"/> class.
        /// </summary>
        public VesselGeoPosition(long shipId, string positionSource, double longitude, double latitude, DateTime timestamp, IGeoHashAlgorithm geoHasher)
        {
            ShipId = shipId;
            Position = new Point(new Position(latitude, longitude));
            Timestamp = timestamp;
            Date = timestamp.Date;
            GeoHash = geoHasher.GeoHashCoordinates(longitude, latitude);
            Source = Enum.Parse<PositionSource>(positionSource);

            CreatedOn = DateTime.Now;
        }

        [JsonProperty("shipId")]
        public long ShipId { get; }
        [JsonProperty("position")]
        public Point Position { get; }
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; }
        [JsonProperty("date")]
        public DateTime Date { get; }
        [JsonProperty("source")]
        public PositionSource Source { get; }
        [JsonProperty("createdon")]
        public DateTime CreatedOn { get; }

        [JsonProperty("geoHash")]
        public string GeoHash { get; }
        [JsonProperty("geoHash_date")]
        public string GeoHash_Date
        {
            // This property is used as the partition-key in the CosmosDB collection.
            get { return $"{GeoHash}_{Date:yyyyMMdd}"; }
        }
    }

    public enum PositionSource
    {
        Ais,
        Lrit,
        Vms
    }
}