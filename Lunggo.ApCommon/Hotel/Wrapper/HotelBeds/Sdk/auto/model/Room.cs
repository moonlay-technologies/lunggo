using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class Room
    {
        public SimpleTypes.BookingStatus status;
        public string code { get; set; }
        public string name { get; set; }
        public List<Pax> paxes { get; set; }
        public List<Rate> rates { get; set; }
    }
}