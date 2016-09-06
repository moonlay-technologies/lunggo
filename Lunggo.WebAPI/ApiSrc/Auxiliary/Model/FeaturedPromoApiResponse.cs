using System.Collections.Generic;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class FeaturedPromoApiResponse : ApiResponseBase
    {
        [JsonProperty("featuredPromos", NullValueHandling = NullValueHandling.Ignore)]
        public List<FeaturedPromo> FeaturedPromos { get; set; }
    }

    public class FeaturedPromo
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("bannerUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string BannerUrl { get; set; }
        [JsonProperty("detailsUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string DetailsUrl { get; set; }  
    }
}