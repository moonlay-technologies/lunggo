﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model.Logic
{
    public class SearchHotelOutput
    {
        [JsonProperty("searchId", NullValueHandling = NullValueHandling.Ignore)]
        public string SearchId { get; set; }
        [JsonProperty("hotels", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelDetailForDisplay> HotelDetailLists { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public int TotalHotel { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public HotelSorting SortingParam { get; set; }
        public HotelFilter FilterParam { get; set; }
    }
}