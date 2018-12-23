using System;

namespace GeoTracking.PositionEnricher
{
    public class PositionReport
    {
        public long ShipId { get; }
        public DateTime Timestamp { get; }
        public double Longitude { get; }
        public double Latitude { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionReport"/> class.
        /// </summary>
        public PositionReport(long shipId, DateTime timestamp, double longitude, double latitude)
        {
            this.ShipId = shipId;
            this.Timestamp = timestamp;
            this.Longitude = longitude;
            this.Latitude = latitude;
        }
    }
}