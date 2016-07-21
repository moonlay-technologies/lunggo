using System.Collections.Generic;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class FeaturePromoApiResponse : ApiResponseBase
    {
        [JsonProperty("featurePromos")]
        public List<FeaturePromo> FeaturePromos { get; set; }
    }

    public class FeaturePromo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("bannerUrl")]
        public string BannerUrl { get; set; }  
    }
}