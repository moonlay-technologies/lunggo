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
        [JsonProperty("facilityCd", NullValueHandling = NullValueHandling.Ignore)]
        public int FacilityCode;
        [JsonProperty("facilityDesc", NullValueHandling = NullValueHandling.Ignore)]
        public string FacilityDescription;
        [JsonProperty("facilityGroupCd", NullValueHandling = NullValueHandling.Ignore)]
        public int FacilityGroupCode;
        [JsonProperty("facilityGroupDesc", NullValueHandling = NullValueHandling.Ignore)]
        public string FacilityGroupDescription;
    }
    
    public class HotelFacility
    {
        [JsonProperty("facilityCd", NullValueHandling = NullValueHandling.Ignore)]
        public int FacilityCode;
        [JsonProperty("facilityGroupCd", NullValueHandling = NullValueHandling.Ignore)]
        public int FacilityGroupCode;
    }
}
