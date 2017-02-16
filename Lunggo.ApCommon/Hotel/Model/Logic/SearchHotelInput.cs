using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Constant;

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
}
