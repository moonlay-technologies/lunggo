using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.Hotel.ViewModels
{
    public class HotelSearchBase
    {
        public int? RoomCount { get; set; }
        public int? GuestCount { get; set; }
        public string StringChkInDate { get; set; }
        public DateTime? ChkInDate { get; set; }
        public string StringChkOutDate { get; set; }
        public DateTime? ChkOutDate { get; set; }
        public int? ChkInDay { get; set; }
        public int? ChkInMonth { get; set; }
        public int? ChkInYear { get; set; }
        public int? StayCount { get; set; }
        public string CountryArea { get; set; }
        public string ProvinceArea { get; set; }
        public string LargeArea { get; set; }
        public string SmallArea { get; set; }
        public long? CountryCode { get; set; }
        public long? ProvinceCode { get; set; }
        public long? LargeCode { get; set; }
        public long? SmallCode { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string Keyword { get; set; }
        public long HotelId { get; set; }
        public string HotelName { get; set; }
    }
}