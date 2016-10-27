using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model.Logic
{
    public class SelectHotelRoomInput
    {
        public string SearchId { get; set; }
        public List<RegsId> RegsIds { get; set; }
    }

    public class RegsId
    {
        [JsonProperty("regsId", NullValueHandling = NullValueHandling.Ignore)]
        public string RegId { get; set; }
        [JsonProperty("rateCount", NullValueHandling = NullValueHandling.Ignore)]
        public int RateCount { get; set; }
        [JsonProperty("adultCount", NullValueHandling = NullValueHandling.Ignore)]
        public int AdultCount { get; set; }
        [JsonProperty("childCount", NullValueHandling = NullValueHandling.Ignore)]
        public int ChildCount { get; set; }
        [JsonProperty("childrenAges", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> ChildrenAges { get; set; } 
    }
}
