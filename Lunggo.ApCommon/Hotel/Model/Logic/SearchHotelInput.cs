using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model.Logic
{
    public class SearchHotelInput
    {
        public SearchHotelType SearchHotelType { get; set; }
        public string SearchId { get; set; }
        public long Location { get; set; }
        public int Nights { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime Checkout { get; set; }
        //public int Rooms { get; set; }
        //public int AdultCount { get; set; }
        //public int ChildCount { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public HotelFilter FilterParam { get; set; }
        public string SortingParam { get; set; }
        public int HotelCode { get; set; }
        public string RegsId { get; set; }
        public List<Occupancy> Occupancies { get; set; }
        public SearchHotelInput()
        {
            FilterParam = new HotelFilter();
        }
    }

    public class PaxDataInput
    {
        [JsonProperty("roomCount")]
        public int RoomCount { get; set; }
        [JsonProperty("adultCount")]
        public int AdultCount { get; set; }
        [JsonProperty("childCount")]
        public int ChildCount { get; set; }
        [JsonProperty("childAges")]
        public List<int> ChildAges { get; set; }
    }
}
