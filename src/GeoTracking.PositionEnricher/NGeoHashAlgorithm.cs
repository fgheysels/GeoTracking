namespace GeoTracking.PositionEnricher
{
    public class NGeoHashAlgorithm : IGeoHashAlgorithm
    {
        private readonly int _precision;

        public static readonly NGeoHashAlgorithm Default = new NGeoHashAlgorithm();

        /// <summary>
        /// Initializes a new instance of the <see cref="NGeoHashAlgorithm"/> class.
        /// </summary>
        public NGeoHashAlgorithm() : this(4)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NGeoHashAlgorithm"/> class.
        /// </summary>
        public NGeoHashAlgorithm(int precision)
        {
            _precision = precision;
        }
        public string GeoHashCoordinates(double longitude, double latitude)
        {
            return NGeoHash.GeoHash.Encode(latitude, longitude, _precision);
        }
    }
}