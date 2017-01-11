using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Service;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model.Logic
{
    public class AvailableRatesOutput : ResultBase
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public int Total { get; set; }
        [JsonProperty("rooms", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelRoomForDisplay> Rooms { get; set; }
    }
}
