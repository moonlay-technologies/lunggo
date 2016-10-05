using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelSearchApiResponse
    {
        public string SearchId { get; set; }
        public int TotalHotel { get; set; }
        public List<HotelDetailForDisplay> Hotels { get; set; } 
    }
}