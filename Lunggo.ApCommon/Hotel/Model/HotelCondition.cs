using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class SearchHotelCondition
    {
        public string Location { get; set; }
        public int Zone { get; set; }
        public int Nights { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime Checkout { get; set; }
        public int Rooms { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        //Sorting
        //Filter1
        //Page Param
    }

    public class HotelRevalidateInfo
    {
        public string RateKey { get; set; }
        public decimal Price { get; set; }
    }

    public class HotelIssueInfo
    {
        public string RsvNo { get; set; }
        public List<Pax> Pax { get; set; }
        public List<HotelRoom> Rooms { get; set; }
        public Contact Contact { get; set; }
        public string SpecialRequest { get; set; }
    }
}
