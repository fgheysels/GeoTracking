using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoTracking.Positions.Api.Configuration
{
    public class CosmosDbSettings
    {
        public static readonly CosmosDbSettings Current = new CosmosDbSettings();

        public string CosmosDbEndpoint { get; set; }
        public string CosmosDbKey { get; set; }
    }
}
