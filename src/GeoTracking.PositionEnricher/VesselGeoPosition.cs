using System;
using GeoJSON.Net.Geometry;

namespace GeoTracking.PositionEnricher
{
    public class VesselGeoPosition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselGeoPosition"/> class.
        /// </summary>
        public VesselGeoPosition(PositionReport position, IGeoHashAlgorithm geoHasher)
        {
            ShipId = position.ShipId;
            Position = new Point(new Position(position.Latitude, position.Longitude));
            Timestamp = position.Timestamp;
            GeoHash = geoHasher.GeoHashCoordinates(position.Longitude, position.Latitude);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselGeoPosition"/> class.
        /// </summary>
        public VesselGeoPosition(long shipId, double longitude, double latitude, DateTime timestamp, IGeoHashAlgorithm geoHasher)
        {
            ShipId = shipId;
            Position = new Point(new Position(latitude, longitude));
            Timestamp = timestamp;
            GeoHash = geoHasher.GeoHashCoordinates(longitude, latitude);
        }

        public long ShipId { get; }
        public Point Position { get; }
        public DateTime Timestamp { get; }

        public string GeoHash { get; }
    }
}