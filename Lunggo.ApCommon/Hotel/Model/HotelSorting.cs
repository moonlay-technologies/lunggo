using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelSorting
    {
        [JsonProperty("lowestPrice")]
        public bool LowestPrice { get; set; }
        [JsonProperty("highestPrice")]
        public bool HighestPrice { get; set; }
        [JsonProperty("highestStar")]
        public bool HighestStar { get; set; }
        [JsonProperty("highestReviewScore")]
        public bool HighestReviewScore { get; set; }
        [JsonProperty("highestPopularity")]
        public bool HighestPopularity { get; set; }
    }
}
