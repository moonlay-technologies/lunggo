using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages
{
    public class BookingListRS : AbstractGenericResponse
    {
        public Bookings bookings { get; set; }
    }
}
