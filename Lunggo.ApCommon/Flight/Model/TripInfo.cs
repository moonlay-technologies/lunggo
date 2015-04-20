using System;

namespace Lunggo.ApCommon.Flight.Model
{
    public class TripInfo
    {
        public string OriginAirport { get; set; }
        public string DestinationAirport { get; set; }
        public DateTime DepartureDate { get; set; }
        public TimeSpan TotalDuration { get; set; }
    }
}