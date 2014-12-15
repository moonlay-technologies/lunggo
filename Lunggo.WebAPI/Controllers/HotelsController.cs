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
            var response = new HotelSearchApiResponse
            {
                SearchId = "dummySearchId",
                InitialRequest = request,
                HotelList = new List<HotelExcerpt>
                {
                    new HotelExcerpt
                    {
                        HotelName = "Hotel Borobudur",
                        HotelId = "456789",
                        Address = "Jalan Lapangan Banteng, Jakarta Pusat",
                        Area = "Gambir",
                        Country = "Indonesia",
                        Province = "DKI Jakarta",
                        StarRating = 5,
                        Latitude = 67,
                        Longitude = 115,
                        ImageUrlList = new List<String>
                        {
                            "http://bestjakartahotels.com/wp-content/dubai_hotel/5-Hotel_Borobudur_Jakarta.jpg",
                            "http://www.cleartrip.com/places/hotels//4373/437368/images/8536640_w.jpg"
                        },
                        LowestPrice = new Price()
                        {
                            Value = 500000,
                            Currency = "IDR"
                        }
                    },
                    new HotelExcerpt
                    {
                        HotelName = "Hotel Sultan",
                        HotelId = "456789",
                        Address = "Jalan Gatot Subroto, Jakarta Pusat",
                        Area = "Senayan",
                        Country = "Indonesia",
                        Province = "DKI Jakarta",
                        StarRating = 5,
                        Latitude = 67,
                        Longitude = 115,
                        ImageUrlList = new List<String>
                        {
                            "http://images.yuktravel.com/images/upload/review/1333112196-The_Sultan_Hotel_Jakarta_Exterior.jpg",
                            "http://data.tribunnews.com/foto/bank/images/hotel-sultan-1.jpg"
                        },
                        LowestPrice = new Price()
                        {
                            Value = 500000,
                            Currency = "IDR"
                        }
                    },
                    new HotelExcerpt
                    {
                        HotelName = "Hotel Sultan",
                        HotelId = "456789",
                        Address = "Jalan Gatot Subroto, Jakarta Pusat",
                        Area = "Senayan",
                        Country = "Indonesia",
                        Province = "DKI Jakarta",
                        StarRating = 5,
                        Latitude = 67,
                        Longitude = 115,
                        ImageUrlList = new List<String>
                        {
                            "http://hikarivoucher.com/files/hotels/547/keraton-at-the-plaza.jpg",
                            "http://assets.keratonattheplazajakarta.com/lps/assets/gallery/lux3635po.126266_lg.jpg"
                        },
                        LowestPrice = new Price()
                        {
                            Value = 500000,
                            Currency = "IDR"
                        }
                    },
                },
                TotalHotelCount = 3 
            };
            return response;
        }
    }

    public class HotelSearchApiRequest
    {
        public int LocationId { get; set; }
        public String StayDate { get; set; }
        public int StayLength { get; set; }
        public String SearchId { get; set; }
        public int SortBy { get; set; }
        public int StartIndex { get; set; }
        public int ResultCount { get; set; }
        public long MinPrice { get; set; }
        public long MaxPrice { get; set; }
        public String StarRating { get; set; }
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

    public class HotelExcerpt
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
        public Price LowestPrice { get; set; }
    }
}
