namespace GeoTracking.PositionEnricher
{
    public interface IGeoHashAlgorithm
    {
        string GeoHashCoordinates(double longitude, double latitude);
    }
}