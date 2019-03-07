using System;

namespace GeoTracking.IngestToDb
{
    class Configuration
    {
        public static Uri GetCosmosDbEndpoint()
        {
            return new Uri(Environment.GetEnvironmentVariable("cosmosDbEndpoint"));
        }

        public static string GetCosmosDbKey()
        {
            return Environment.GetEnvironmentVariable("cosmosDbKey");
        }
    }
}
