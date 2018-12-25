using System;

namespace GeoTracking.PositionEnricher
{
   static class Configuration
    {
        public static string VesselGeoPositionConnectionString()
        {
            return Environment.GetEnvironmentVariable("geoPositionEventHubConnection");
        }
    }
}
