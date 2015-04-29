using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class GetDetailsInput
    {
        public string BookingId { get; set; }
        public List<FlightTripInfo> TripInfos { get; set; }
    }
}
