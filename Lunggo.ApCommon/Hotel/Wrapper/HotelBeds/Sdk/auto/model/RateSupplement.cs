using System;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class RateSupplement
    {
        public string code { get; set; }
        public string name { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public decimal amount { get; set; }
        public int nights { get; set; }
        public int paxNumber { get; set; }
    }
}
