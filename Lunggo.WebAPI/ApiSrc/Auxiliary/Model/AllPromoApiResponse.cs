using System;
using System.Collections.Generic;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class AllPromoApiResponse : ApiResponseBase
    {
        [JsonProperty("allPromos")]
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

   public enum PromoType
    {
        CouponCode = 0,
        Instalment = 1,
        Discount = 2,
        Other = 99
    }
}