using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelRoomFacilities
    {
        [JsonProperty("facilityCode", NullValueHandling = NullValueHandling.Ignore)]
        public int FacilityCode;
        [JsonProperty("facilityGroupCode", NullValueHandling = NullValueHandling.Ignore)]
        public int FacilityGroupCode;
    }
}
