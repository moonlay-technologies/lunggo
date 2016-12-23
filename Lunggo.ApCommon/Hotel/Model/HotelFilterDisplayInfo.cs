using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery.EquationParser.Implementation;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelFilterDisplayInfo
    {
        [JsonProperty("accomodationFilter", NullValueHandling = NullValueHandling.Ignore)]
        public List<AccomodationFilterInfo> AccomodationFilter { get; set; }
        [JsonProperty("facilityFilter", NullValueHandling = NullValueHandling.Ignore)]
        public List<FacilitiesFilterInfo> FacilityFilter { get; set; }
        [JsonProperty("zoneFilter", NullValueHandling = NullValueHandling.Ignore)]
        public List<ZoneFilterInfo> ZoneFilter { get; set; }
        [JsonProperty("starFilter", NullValueHandling = NullValueHandling.Ignore)]
        public List<StarFilterInfo> StarFilter { get; set; }
        [JsonProperty("areaFilter", NullValueHandling = NullValueHandling.Ignore)]
        public List<AreaFilterInfo> AreaFilter { get; set; } 
    }

    public class AreaFilterInfo
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int Count { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
    }

    public class AccomodationFilterInfo
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int Count { get; set; }
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class FacilitiesFilterInfo
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int Count { get; set; }
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class ZoneFilterInfo
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int Count { get; set; }
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class StarFilterInfo
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int Count { get; set; }
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }
}
