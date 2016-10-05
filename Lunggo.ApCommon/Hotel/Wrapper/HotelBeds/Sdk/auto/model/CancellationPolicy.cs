using System;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class CancellationPolicy
    {
        public decimal amount { get; set; }
        public decimal hotelAmount { get; set; }
        public string hotelCurrency { get; set; }
        public DateTime from { get; set; }
    }
}
