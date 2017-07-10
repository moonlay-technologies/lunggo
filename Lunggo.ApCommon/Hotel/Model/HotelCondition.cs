using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class SearchHotelCondition
    {
        public string Destination { get; set; }
        public string Zone { get; set; }
        public string Area { get; set; }
        public string Country { get; set; }
        public int HotelCode { get; set; }
        public int Nights { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime Checkout { get; set; }
        public int Rooms { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public string SearchId { get; set; }
        public List<Occupancy> Occupancies { get; set; } 
        //Sorting
        //Filter1
        //Page Param
    }

    public class Occupancy
    {
        [JsonProperty("roomCount")]
        public int RoomCount { get; set; }
        [JsonProperty("adultCount")]
        public int AdultCount { get; set; }
        [JsonProperty("childCount")]
        public int ChildCount { get; set; }
        [JsonProperty("childrenAges")]
        public List<int> ChildrenAges { get; set; } 
    }
    public class HotelRevalidateInfo
    {
        public string RateKey { get; set; }
        public decimal Price { get; set; }
    }

    public class HotelBookInfo
    {
        public int Nights { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime Checkout { get; set; }
        public int Rooms { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public string RoomId { get; set; }
        public string HotelName { get; set; }
        public string Token { get; set; }
    }

    public class HotelIssueInfo
    {
        public string RsvNo { get; set; }
        public List<Pax> Pax { get; set; }
        public List<HotelRoom> Rooms { get; set; }
        public Contact Contact { get; set; }
        public string SpecialRequest { get; set; }
    }

    public class HotelCancelBookingInfo
    {
        public string BookingReference { get; set; }
    }
}
