using System.Collections.Generic;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class Bookings
    {
        public List<Booking> bookings { get; set; }
        public int from { get; set; }
        public int to { get; set; }
        public decimal total { get; set; }
    }
}
