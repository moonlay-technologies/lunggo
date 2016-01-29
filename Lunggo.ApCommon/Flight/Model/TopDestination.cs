using Lunggo.ApCommon.Model;

namespace Lunggo.ApCommon.Flight.Model
{
    public class TopDestination
    {
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public Price CheapestPrice { get; set; }
    }
}
