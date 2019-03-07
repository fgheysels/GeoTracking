using System;

namespace GeoTracking.PositionEnricher
{
    public class PositionReport
    {
        public long ObjectId { get; }
        public string Source { get; }
        public DateTime Timestamp { get; }
        public double Longitude { get; }
        public double Latitude { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionReport"/> class.
        /// </summary>
        public PositionReport(long objectId, string source, DateTime timestamp, double longitude, double latitude)
        {
            this.ObjectId = objectId;
            this.Source = source;
            this.Timestamp = timestamp;
            this.Longitude = longitude;
            this.Latitude = latitude;
        }
    }
}