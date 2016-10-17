using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;

namespace Lunggo.ApCommon.Hotel.Model.Logic
{
    public class SearchHotelInput
    {
        public string SearchId { get; set; }
        public string Location { get; set; }
        public int? Zone { get; set; }
        public int Nights { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime Checkout { get; set; }
        public int Rooms { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public HotelFilter FilterParam { get; set; }
        public HotelSorting SortingParam { get; set; }
        public int HotelCode { get; set; }
        public SearchHotelInput()
        {
            SortingParam = new HotelSorting();
            FilterParam = new HotelFilter();
        }
    }
}