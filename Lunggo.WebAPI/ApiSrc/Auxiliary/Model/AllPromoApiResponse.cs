using System;
using System.Collections.Generic;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class AllPromoApiResponse : ApiResponseBase
    {
        [JsonProperty("allPromos", NullValueHandling = NullValueHandling.Ignore)]
        public List<AllPromo> AllPromos { get; set; }
    }

    public class AllPromo
    {
        [JsonProperty("bookingPeriod", NullValueHandling = NullValueHandling.Ignore)]
        public string BookingPeriod{ get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("travelPeriod", NullValueHandling = NullValueHandling.Ignore)]
        public string TravelPeriod { get; set; }
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("promoType", NullValueHandling = NullValueHandling.Ignore)]
        public PromoType PromoType { get; set; }
        [JsonProperty("bannerUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string BannerUrl { get; set; }    
    }

   public enum PromoType
    {
        CouponCode = 0,
        Instalment = 1,
        Discount = 2,
        Other = 99
    }
}