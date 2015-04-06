using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Model;

namespace Lunggo.CustomerWeb.WebSrc.UW200.Model
{
    public class Uw200HotelDetail
    {
        public int HotelId { get; set; }
        public String HotelName { get; set; }
        public String Address { get; set; }
        public String Country { get; set; }
        public String Area { get; set; }
        public String Province { get; set; }
        public int StarRating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsLatLongSet { get; set; }
        public IEnumerable<HotelDescription> HotelDescription { get; set; }
        public Price LowestPrice { get; set; }
        public IEnumerable<HotelImage> ImageUrlList { get; set; }
        public IEnumerable<HotelFacility> Facilities { get; set; } 
    }
}