using System.Collections.Generic;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class PromoDetailsApiResponse : ApiResponseBase
    {
        [JsonProperty("promoDetails", NullValueHandling = NullValueHandling.Ignore)]
        public PromoDetails PromoDetails { get; set; }
    }

    public class PromoDetails
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("tnc", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Tnc { get; set; }
        [JsonProperty("bannerUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string BannerUrl { get; set; }
        [JsonProperty("hotelPromo", NullValueHandling = NullValueHandling.Ignore)]
        public Hotels Hotels { get; set; }
        [JsonProperty("flightPromo", NullValueHandling = NullValueHandling.Ignore)]
        public Flights Flights { get; set; }
    }

    public class Hotels
    {
        [JsonProperty("promoCode", NullValueHandling = NullValueHandling.Ignore)]
        public string PromoCode { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("hotelChoices", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelChoice> HotelChoices { get; set; }
    }

    public class HotelChoice
    {
        [JsonProperty("place", NullValueHandling = NullValueHandling.Ignore)]
        public string Place { get; set; }
        [JsonProperty("hotels", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Hotels { get; set; }
        [JsonProperty("roomTypes", NullValueHandling = NullValueHandling.Ignore)]
        public string[] RoomType { get; set; }
        [JsonProperty("stayDuration", NullValueHandling = NullValueHandling.Ignore)]
        public string StayDuration { get; set; }
        [JsonProperty("stayPeriod", NullValueHandling = NullValueHandling.Ignore)]
        public string StayPeriod { get; set; }
        [JsonProperty("bookingPeriod", NullValueHandling = NullValueHandling.Ignore)]
        public string BookingPeriod { get; set; }
    }

    public class Flights
    {
        [JsonProperty("travelPeriod", NullValueHandling = NullValueHandling.Ignore)]
        public string TravelPeriod { get; set; }
        [JsonProperty("bookingPeriod", NullValueHandling = NullValueHandling.Ignore)]
        public string BookingPeriod { get; set; }
        [JsonProperty("promoCode", NullValueHandling = NullValueHandling.Ignore)]
        public string PromoCode { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }

    //public class FlightList
    //{
    //    [JsonProperty("airlines")]
    //    public string[] Airlines { get; set; }
    //    [JsonProperty("origins")]
    //    public string[] Origins { get; set; }
    //    [JsonProperty("destinations")]
    //    public string[] Destinations { get; set; }
    //}
}