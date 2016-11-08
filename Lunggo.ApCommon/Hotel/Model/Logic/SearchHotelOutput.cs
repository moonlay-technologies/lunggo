using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model.Logic
{
    public class SearchHotelOutput : ResultBase
    {
        [JsonProperty("searchId", NullValueHandling = NullValueHandling.Ignore)]
        public string SearchId { get; set; }
        [JsonProperty("hotels", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelDetailForDisplay> HotelDetailLists { get; set; }
        public HotelRoomForDisplay HotelRoom { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public int ReturnedHotelCount { get; set; }
        public int TotalHotelCount { get; set; }
        public int FilteredHotelCount { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public bool IsSpecificHotel { get; set; }
        public int? HotelCode { get; set; }
        public HotelFilterDisplayInfo HotelFilterDisplayInfo { get; set; }
    }
}
