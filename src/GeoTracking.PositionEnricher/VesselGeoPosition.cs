using System;
using GeoJSON.Net.Geometry;

namespace GeoTracking.PositionEnricher {
    public class VesselGeoPosition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselGeoPosition"/> class.
        /// </summary>
        public VesselGeoPosition(PositionReport position)
        {
            ShipId = position.ShipId;
            Position = new Point(new Position(position.Latitude, position.Longitude));
            Timestamp = position.Timestamp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselGeoPosition"/> class.
        /// </summary>
        public VesselGeoPosition(long shipId, double longitude, double latitude, DateTime timestamp)
        {
            ShipId = shipId;
            Position = new Point(new Position(latitude, longitude));
            Timestamp = timestamp;
        }

        public long ShipId { get; }
        public Point Position { get; }
        public DateTime Timestamp { get; }
    }
}