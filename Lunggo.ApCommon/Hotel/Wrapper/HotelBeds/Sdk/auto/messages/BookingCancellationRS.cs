using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages
{
    public class BookingCancellationRS : AbstractGenericResponse
    {
        public Booking booking { get; set; }
    }
}
