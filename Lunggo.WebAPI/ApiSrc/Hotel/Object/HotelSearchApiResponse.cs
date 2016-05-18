using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Object
{
    public class HotelSearchApiResponse
    {
        public IEnumerable<HotelExcerpt> HotelList { get; set; }
        public int TotalHotelCount { get; set; }
        public int TotalFilteredCount { get; set; }
        public HotelSearchApiRequest InitialRequest { get; set; }
        public String SearchId { get; set; }
    }
}