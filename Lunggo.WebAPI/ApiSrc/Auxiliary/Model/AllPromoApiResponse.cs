using System;
using System.Collections.Generic;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class AllPromoApiResponse : ApiResponseBase
    {
        [JsonProperty("promos")]
        public List<AllPromo> AllPromos { get; set; }
    }

    public class AllPromo
    {
        [JsonProperty("bookingPeriod")]
        public string BookingPeriod{ get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("travelPeriod")]
        public string TravelPeriod { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("promoType")]
        public PromoType PromoType { get; set; }
        [JsonProperty("bannerUrl")]
        public string BannerUrl { get; set; }    
    }

   public class Hotels
    {
        [JsonProperty("promoCode")]
        public string PromoCode { get; set; } 
        [JsonProperty("description")]
        public DateTime Description { get; set; }
        [JsonProperty("hotels")]
        public List<HotelList> HotelList{ get; set; }
    }

    public class HotelList
    {
        [JsonProperty("place")]
        public string Place { get; set; }
        [JsonProperty("hotel")]
        public string[] Hotels { get; set; }
        [JsonProperty("roomType")]
        public string RoomType { get; set; }
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

    public enum PromoType
    {
        CouponCode = 0,
        Instalment = 1,
        Discount = 2,
        Other = 99
    }
}