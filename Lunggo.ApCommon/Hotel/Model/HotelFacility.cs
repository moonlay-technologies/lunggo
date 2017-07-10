using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{

    public class HotelFacilityForDisplay
    {
        [JsonProperty("general", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> General { get; set; }
        [JsonProperty("sport", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Sport { get; set; }
        [JsonProperty("meal", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Meal { get; set; }
        [JsonProperty("business", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Business { get; set; }
        [JsonProperty("entertainment", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Entertainment { get; set; }
        [JsonProperty("health", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Health { get; set; }
        [JsonProperty("other", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Other { get; set; }
    }
    
    public class HotelFacility
    {
        [JsonProperty("facilityCd", NullValueHandling = NullValueHandling.Ignore)]
        public int FacilityCode { get; set; }
        [JsonProperty("facilityGroupCd", NullValueHandling = NullValueHandling.Ignore)]
        public int FacilityGroupCode { get; set; }
        [JsonProperty("fullCd", NullValueHandling = NullValueHandling.Ignore)]
        public string FullFacilityCode { get; set; }
        [JsonProperty("mustDisplay", NullValueHandling = NullValueHandling.Ignore)]
        public bool MustDisplay;
        [JsonProperty("isFree", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsFree;
        [JsonProperty("isAvailable", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsAvailable;
        [JsonProperty("facilityName", NullValueHandling = NullValueHandling.Ignore)]
        public string FacilityName;
    }
}
