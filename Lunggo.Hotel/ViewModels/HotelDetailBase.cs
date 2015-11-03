using System.Collections.Generic;

namespace Lunggo.Hotel.ViewModels
{
    public class HotelDetailBase
    {
        public long HotelId { get; set; }
        public string HotelName { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string CountryArea { get; set; }
        public string ProvinceArea { get; set; }
        public string LargeArea { get; set; }
        public string SmallArea { get; set; }
        public long? CountryCode { get; set; }
        public long? ProvinceCode { get; set; }
        public long? LargeCode { get; set; }
        public long? SmallCode { get; set; }
        public string Address { get; set; }
        public int HotelStar { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<string> ListPhotoUrl { get; set; }
        public HotelFacilities Facilities { get; set; }
        public List<HotelRoomDetailBase> ListRooms { get; set; }
        public decimal? MinimumPrice { get; set; }
        public double? Discount { get; set; }
    }
}
