using System;
using System.Net.Http;
using Xunit;

namespace GeoTracking.PositionEnricher.Tests
{
    public class PositionProcessorFacts
    {
        [Fact]
        public void CanProcessEventData()
        {
            var report = new PositionReport(18, new DateTime(2018, 12, 14, 10, 29, 54), 55.4436546, 3.6985412);

            var processor = new PositionProcessor();

            var result = processor.ProcessMessage(report);

            Assert.NotNull(result);
            Assert.Equal(report.Longitude, result.Position.Coordinates.Longitude);
            Assert.Equal(report.Latitude, result.Position.Coordinates.Latitude);
        }
    }
}
