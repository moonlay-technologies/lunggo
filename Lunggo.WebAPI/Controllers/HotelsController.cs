using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Lunggo.WebAPI.Controllers
{

    public class HotelsController : ApiController
    {
        [EnableCors(origins: "http://localhost", headers: "*", methods: "*")]
        public HotelSearchApiResponse GetHotels([FromUri] HotelSearchApiRequest request)
        {
            var hotelList = DummyLogic.GetHotels(request);
            var response = new HotelSearchApiResponse
            {
                SearchId = "dummySearchId",
                InitialRequest = request,
                HotelList = hotelList,
                TotalHotelCount = 3
            };

            return response;
        }
    }

    public class HotelSearchApiRequest
    {
        public int LocationId { get; set; }
        public String StayDate { get; set; }
        public String StayLength { get; set; }
        public String SearchId { get; set; }
        public String SortBy { get; set; }
        public String StartIndex { get; set; }
        public String ResultCount { get; set; }
        public String MinPrice { get; set; }
        public String MaxPrice { get; set; }
        public String StarRating { get; set; }
        public String Lang { get; set; }
    }

    public class Price
    {
        public double Value { get; set; }
        public String Currency { get; set; }
    }

    public class HotelSearchApiResponse
    {
        public IEnumerable<HotelExcerpt> HotelList { get; set; }
        public int TotalHotelCount { get; set; }
        public HotelSearchApiRequest InitialRequest { get; set; }
        public String SearchId { get; set; }
    }

    public class HotelDetailBase
    {
        public String HotelName { get; set; }
        public String HotelId { get; set; }
        public String Address { get; set; }
        public int StarRating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public String Country { get; set; }
        public String Province { get; set; }
        public String Area { get; set; }
        public List<String> ImageUrlList { get; set; }
    }



    public class HotelDetailComplete : HotelDetailBase
    {
        public IEnumerable<Passage> HotelDescription { get; set; }
        public Price LowestPrice { get; set; }
    }

    public class Passage
    {
        public String Value { get; set; }
        public String Lang { get; set; }
    }

    public class HotelExcerpt : HotelDetailBase
    {
        public Price LowestPrice { get; set; }
    }

    public enum HotelSearchSortType
    {
        PriceAscending = 1,
        PriceDescending = 2,
        StarRatingAscending = 3,
        StarRatingDescending = 4,
        AlphanumericAscending = 5,
        AlphanumericDescending = 6
    }
}
