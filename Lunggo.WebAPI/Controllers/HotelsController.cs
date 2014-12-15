using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lunggo.WebAPI.Controllers
{
    public class HotelsController : ApiController
    {
        public IEnumerable<HotelSearchResultViewModel> GetHotels([FromUri] SimpleModel model)
        {
            /*var list = new List<String>
            {
                model.StayDate, 
                model.LocationId.ToString(CultureInfo.InvariantCulture)
            };*/
            return list;
        }
    }

    public class SimpleModel
    {
        public int LocationId { get; set; }
        public String StayDate { get; set; }
    }

    public class Price
    {
        public double Value { get; set; }
        public String Currency { get; set; }
    }

    public class HotelSearchResultViewModel
    {
        public String HotelId { get; set; }
        public String HotelName { get; set; }
        public String HotelDescription { get; set; }
        public int StarRating { get; set; }
        public String Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public String Country { get; set; }
        public String City { get; set; }
        public String Area { get; set; }
        public Price LowestRoomPrice { get; set; }
    }
}
