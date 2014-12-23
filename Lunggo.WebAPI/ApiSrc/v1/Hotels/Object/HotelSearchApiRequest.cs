using System;

namespace Lunggo.WebAPI.ApiSrc.v1.Hotels.Object
{
    public class HotelSearchApiRequest
    {
        public int LocationId { get; set; }
        public String StayDate { get; set; }
        public String StayLength { get; set; }
        public int RoomCount { get; set; }
        public String SearchId { get; set; }
        public String SortBy { get; set; }
        public String StartIndex { get; set; }
        public String ResultCount { get; set; }
        public String MinPrice { get; set; }
        public String MaxPrice { get; set; }
        public String StarRating { get; set; }
        public String Lang { get; set; }
    }
}