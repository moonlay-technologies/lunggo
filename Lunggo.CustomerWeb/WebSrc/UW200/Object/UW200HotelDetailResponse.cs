using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Model;

namespace Lunggo.CustomerWeb.WebSrc.UW200.Object
{
    public class Uw200HotelDetailResponse
    {
        public int HotelId { get; set; }
        public DateTime StayDate { get; set; }
        public int StayLength { get; set; }
        public String SearchId { get; set; }
        public String Lang { get; set; }
        public IEnumerable<RoomOccupant> RoomOccupants { get; set; }
        public String HotelName { get; set; }
        public String Address { get; set; }
        public int StarRating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public String Country { get; set; }
        public String Province { get; set; }
        public String Area { get; set; }
        public IEnumerable<HotelImage> ImageUrlList { get; set; }
        public I18NText HotelDescription { get; set; }
        public Price LowestPrice { get; set; }
    }
}