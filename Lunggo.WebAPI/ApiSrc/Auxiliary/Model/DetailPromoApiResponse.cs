using System.Collections.Generic;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class DetailPromoApiResponse : ApiResponseBase
    {
        [JsonProperty("detailPromo", NullValueHandling = NullValueHandling.Ignore)]
        public DetailPromo DetailPromo { get; set; }
    }

    public class DetailPromo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("tnc")]
        public string[] Tnc { get; set; }
        [JsonProperty("bannerUrl")]
        public string BannerUrl { get; set; }
        [JsonProperty("hotelPromo")]
        public Hotels Hotels { get; set; }
        [JsonProperty("flightPromo")]
        public Flights Flights { get; set; }
    }

    public class Hotels
    {
        [JsonProperty("promoCode")]
        public string PromoCode { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("hotelChoices")]
        public List<HotelChoice> HotelChoices { get; set; }
    }

    public class HotelChoice
    {
        [JsonProperty("place")]
        public string Place { get; set; }
        [JsonProperty("hotels")]
        public string[] Hotels { get; set; }
        [JsonProperty("roomTypes")]
        public string[] RoomType { get; set; }
        [JsonProperty("stayDuration")]
        public string StayDuration { get; set; }
        [JsonProperty("stayPeriod")]
        public string StayPeriod { get; set; }
        [JsonProperty("bookingPeriod")]
        public string BookingPeriod { get; set; }
    }

    public class Flights
    {
        [JsonProperty("travelPeriod")]
        public string TravelPeriod { get; set; }
        [JsonProperty("bookingPeriod")]
        public string BookingPeriod { get; set; }
        [JsonProperty("promoCode")]
        public string PromoCode { get; set; }
        [JsonProperty("description")]
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