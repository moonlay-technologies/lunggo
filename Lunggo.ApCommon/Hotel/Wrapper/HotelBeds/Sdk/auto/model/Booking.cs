using System;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class Booking
    {
        public string reference { get; set; }
        public string cancellationReference { get; set; }
        public string clientReference { get; set; } 
	    public DateTime creationDate { get; set; }
        public SimpleTypes.BookingStatus status { get; set; }    
	    public decimal agCommision { get; set; }    
	    public decimal commisionVAT { get; set; }
        public string creationUser { get; set; }
        public Holder holder { get; set; }
        public Hotel hotel { get; set; }
        public string remark { get; set; }
    }
}
