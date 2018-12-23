using System;
using Newtonsoft.Json;
using Xunit;

namespace GeoTracking.PositionEnricher.Tests
{
    public class VesselGeoPositionTests
    {
        [Fact]
        public void CanCorrectlyJsonSerializeVesselGeoPosition()
        {
            var geoPosition = new VesselGeoPosition(89, 55.4318965466, -3.698865, new DateTime(2018, 12, 23, 22, 28, 44));

            var serialized = JsonConvert.SerializeObject(geoPosition);

            Assert.Equal("{\"ShipId\":89,\"Position\":{\"type\":\"Point\",\"coordinates\":[55.4318965466,-3.698865]},\"Timestamp\":\"2018-12-23T22:28:44\"}", serialized);
        }
    }
}
