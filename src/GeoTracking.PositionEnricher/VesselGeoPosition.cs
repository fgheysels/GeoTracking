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
        }

        public long ShipId { get; }
        public Point Position { get; }
        public DateTime Timestamp { get; }
        public DateTime Date { get; }
        public PositionSource Source { get; }

        public string GeoHash { get; }
    }

    public enum PositionSource
    {
        Ais,
        Lrit,
        Vms
    }
}