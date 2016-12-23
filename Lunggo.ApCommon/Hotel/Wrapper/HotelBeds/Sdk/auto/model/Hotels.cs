using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class Hotels
    {
        public List<Hotel> hotels;
        public DateTime checkIn { get; set; }
        public DateTime checkOut { get; set; }
        public int total { get; set; }
    }
}
