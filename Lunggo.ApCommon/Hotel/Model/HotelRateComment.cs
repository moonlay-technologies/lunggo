using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelRateComment
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set;}
        [JsonProperty("incoming", NullValueHandling = NullValueHandling.Ignore)]
        public int Incoming { get; set; }
        [JsonProperty("hotelCd", NullValueHandling = NullValueHandling.Ignore)]
        public int HotelCode { get; set; }
        [JsonProperty("ratelCd", NullValueHandling = NullValueHandling.Ignore)]
        public int RateCode { get; set; }
        [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DateEnd { get; set; }
        [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DateStart { get; set; }
        [JsonProperty("desc", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

    //    [JsonProperty("CommentByRates", NullValueHandling = NullValueHandling.Ignore)]
    //    public List<CommentsByRates> CommentsByRates { get; set; }
    }

    //public class CommentsByRates
    //{
    //    [JsonProperty("RateCds", NullValueHandling = NullValueHandling.Ignore)]
    //    public List<int> RateCodes { get; set; }
    //    [JsonProperty("comments", NullValueHandling = NullValueHandling.Ignore)]
    //    public List<Comment> Comments { get; set; }
    //}

    //public class Comment
    //{
    //    [JsonProperty("end", NullValueHandling = NullValueHandling.Ignore)]
    //    public string DateEnd { get; set; }
    //    [JsonProperty("start", NullValueHandling = NullValueHandling.Ignore)]
    //    public string DateStart { get; set; }
    //    [JsonProperty("desc", NullValueHandling = NullValueHandling.Ignore)]
    //    public string Description { get; set; }
    //}
}
