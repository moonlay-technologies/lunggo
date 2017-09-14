using Newtonsoft.Json;
using Lunggo.ApCommon.Product.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;

namespace Lunggo.WebAPI.ApiSrc.Savepax.Model
{
    public class SavepaxApiRequest : ApiRequestBase
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("paxfordisplay", NullValueHandling = NullValueHandling.Ignore)]
        public PaxForDisplay PaxForDisplay { get; set; }
    }
}